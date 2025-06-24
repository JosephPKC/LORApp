using LORCardLoader.Loader.Db;
using LORCardLoader.Loader.Inserts;
using LORCardLoader.Loader.Parsing;
using LORCardLoader.Models;
using System.Data.SQLite;
using System.Reflection;

namespace LORCardLoader.Loader;

internal class CardLoader(IDbController pDb, IModelParser pParser)
{
    private static readonly string[] _jsons = [
        "globals-en_us.json",
        "set1-en_us.json",
        "set2-en_us.json",
        "set3-en_us.json",
        "set4-en_us.json",
        "set5-en_us.json",
        "set6cde-en_us.json",
        "set7-en_us.json",
        "set7b-en_us.json",
        "set8-en_us.json",
    ];

    private static readonly SchemaTableKeys[] _tableKeys = [
        SchemaTableKeys.CardAssocCardLink,
        SchemaTableKeys.CardKeywordLink,
        SchemaTableKeys.CardRegionLink,
        SchemaTableKeys.CardSubtypeLink,
        SchemaTableKeys.ChampionCards,
        SchemaTableKeys.SpellCards,
        SchemaTableKeys.UnitCards,
        SchemaTableKeys.Cards,
        SchemaTableKeys.Keywords,
        SchemaTableKeys.Regions
    ];

    private readonly IDbController _db = pDb;
    private readonly IModelParser _parser = pParser;

    public void CleanLoadIntoDb(string pJsonDir)
    {
        List<string> missingFiles = GetMissingFiles(pJsonDir);
        if (missingFiles.Count > 0)
        {
            throw new Exception($"Missing files in {pJsonDir}: {string.Join(", ", missingFiles)}");
        }

        ResetAllTables();
        LoadGlobals(pJsonDir);
        //LoadAllSets(pJsonDir);
    }

    private List<string> GetMissingFiles(string pJsonDir) {
        List<string> missingFiles = [];
        foreach (string json in _jsons)
        {
            Console.WriteLine($"Checking File: {pJsonDir}\\{json}");
            if (!File.Exists($"{pJsonDir}\\{json}"))
            {
                missingFiles.Add(json);
            }
        }

        return missingFiles;
    }

    private void ResetAllTables()
    {
        foreach (SchemaTableKeys table in _tableKeys)
        {
            ResetTable(table);
        }
    }

    private void LoadGlobals(string pJsonDir)
    {
        string jsonFile = $"{pJsonDir}\\{_jsons[0]}";
        GlobalModel? globals = _parser.GetModel<GlobalModel>(jsonFile) ??
            throw new Exception($"ERROR: Could not load the globals from {jsonFile}.");

        HashSet<string> keywords = [];
        foreach (var terms in globals.VocabTerms)
        {
            keywords.Add(terms.NameRef.ToUpper());
        }

        foreach (var keys in globals.Keywords)
        {
            if (keywords.Contains(keys.NameRef.ToUpper()))
            {
                Console.WriteLine($"Found Hit: {keys.NameRef}");
            }
        }

        //  Load the keywords
        Console.WriteLine($"Loading Keyword Globals");
        IInsertBuilder<KeywordModel> keywordQueryBuilder = InsertBuilderFactory.CreateInsertBuilder<KeywordModel>();
        //InsertAll(SchemaTableKeys.Keywords, globals.VocabTerms, keywordQueryBuilder);
        InsertAll(SchemaTableKeys.Keywords, globals.Keywords, keywordQueryBuilder);
        //  Load the regions
        Console.WriteLine($"Loading Region Globals");
        IInsertBuilder<RegionModel> regionQueryBuilder = InsertBuilderFactory.CreateInsertBuilder<RegionModel>();
        InsertAll(SchemaTableKeys.Regions, globals.Regions, regionQueryBuilder);
    }

    private void LoadAllSets(string pJsonDir)
    {
        foreach (string json in _jsons)
        {
            LoadSet(pJsonDir, json);
        }
    }

    private void LoadSet(string pJsonDir, string pSetJson)
    {
        string jsonFile = $"{pJsonDir}\\{pSetJson}";
        IEnumerable<CardModel>? cards = _parser.GetModels<CardModel>(jsonFile);
        if (cards is null || !cards.Any()) {
            throw new Exception($"ERROR: Could not load the cards from {jsonFile}.");
        }

        foreach (CardModel card in cards) {
            switch (card.Type.ToUpper())
            {
                case "CHAMPION":
                    InsertCard(card);
                    Insert(SchemaTableKeys.ChampionCards, card, InsertBuilderFactory.CreateCardInsertBuilder(card.Type));
                    break;
                case "SPELL":
                    InsertCard(card);
                    Insert(SchemaTableKeys.SpellCards, card, InsertBuilderFactory.CreateCardInsertBuilder(card.Type));
                    break;
                case "UNIT":
                    InsertCard(card);
                    Insert(SchemaTableKeys.UnitCards, card, InsertBuilderFactory.CreateCardInsertBuilder(card.Type));
                    break;
                default:
                    Console.WriteLine($"{card.Type} is not supported.");
                    break;
            }
        }
    }

    private void InsertCard(CardModel pCard)
    {
        Console.WriteLine($"INSERT {pCard.CardCode} - {pCard.Name} - {pCard.Type}");
        Insert(SchemaTableKeys.Cards, pCard, InsertBuilderFactory.CreateInsertBuilder<CardModel>());

        foreach (string assocCard in pCard.AssociatedCards)
        {
            InsertLink(SchemaTableKeys.CardAssocCardLink, [pCard.CardCode, assocCard]);
        }

        foreach (string keyword in pCard.KeywordRefs)
        {
            InsertLink(SchemaTableKeys.CardKeywordLink, [pCard.CardCode, keyword]);
        }

        foreach (string region in pCard.RegionRefs)
        {
            InsertLink(SchemaTableKeys.CardRegionLink, [pCard.CardCode, region]);
        }

        foreach (string subtype in pCard.Subtypes)
        {
            InsertLink(SchemaTableKeys.CardSubtypeLink, [pCard.CardCode, subtype]);
        }
    }

    private void ResetTable(SchemaTableKeys pTableKey)
    {
        Console.WriteLine($"DROP AND CREATE {pTableKey}");
        if (!DbSchema.Config.TryGetValue(pTableKey, out SchemaConfig? config) || config is null)
        {
            //  No table key found.
            Console.WriteLine($"WARNING: {pTableKey} not found in the schema config. Skipping.");
            return;
        }

        try
        {
            _db.Drop(config.Table);
        }
        catch (SQLiteException ex)
        {
            if (!ex.Message.Contains("NO SUCH TABLE", StringComparison.CurrentCultureIgnoreCase))
            {
                throw;
            }
            else
            {
                Console.WriteLine($"{pTableKey} does not exist. Skipping.");
            }
        }
        _db.Create(config.Table, config.CreateQueries);
    }

    private void Insert<TModel>(SchemaTableKeys pTableKey, TModel pModel, IInsertBuilder<TModel> pQueryBuilder) where TModel : BaseModel
    {
        if (!DbSchema.Config.TryGetValue(pTableKey, out SchemaConfig? config) || config is null)
        {
            //  No table key found.
            Console.WriteLine($"WARNING: {pTableKey} not found in the schema config. Skipping.");
            return;
        }

        _db.Insert(config.Table, pQueryBuilder.BuildInsertValues(pModel));
    }

    private void InsertLink(SchemaTableKeys pTableKey, IEnumerable<string> pLinkValues) 
    {
        if (!DbSchema.Config.TryGetValue(pTableKey, out SchemaConfig? config) || config is null)
        {
            //  No table key found.
            Console.WriteLine($"WARNING: {pTableKey} not found in the schema config. Skipping.");
            return;
        }

        _db.Insert(config.Table, pLinkValues);
    }

    private void InsertAll<TModel>(SchemaTableKeys pTableKey, IEnumerable<TModel> pModels, IInsertBuilder<TModel> pQueryBuilder) where TModel : BaseModel
    {
        if (!DbSchema.Config.TryGetValue(pTableKey, out SchemaConfig? config) || config is null)
        {
            //  No table key found.
            Console.WriteLine($"WARNING: {pTableKey} not found in the schema config. Skipping.");
            return;
        }

        List<IEnumerable<string>> valuesToInsert = [];
        foreach (TModel model in pModels) {
            valuesToInsert.Add(pQueryBuilder.BuildInsertValues(model));
        }
        _db.InsertBatch(config.Table, valuesToInsert);
    }
}