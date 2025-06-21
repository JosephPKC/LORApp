using System.Text.Json;
using LORCardLoader.Loader.Db;
using LORCardLoader.Loader.InsertQueries;
using LORCardLoader.Models;

namespace LORCardLoader.Loader;

internal class CardLoader
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

    private readonly IDbController _db;

    public CardLoader(IDbController pDb)
    {
        _db = pDb;
    }

    public void CleanLoadIntoDb(string pJsonDir)
    {
        List<string> missingFiles = GetMissingFiles(pJsonDir);
        if (missingFiles.Count > 0)
        {
            throw new Exception($"Missing files in {pJsonDir}: {string.Join(", ", missingFiles)}");
        }

        ResetAllTables();
        LoadGlobals(pJsonDir);


        
    }

    private List<string> GetMissingFiles(string pJsonDir) {
        List<string> missingFiles = [];
        foreach (string json in _jsons)
        {
            if (!File.Exists($"pJsonDir/{json}"))
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
        string jsonFile = $"{pJsonDir}/{_jsons[0]}";
        GlobalModel? globals = GetModel<GlobalModel>(jsonFile) ??
            throw new Exception($"ERROR: Could not load the globals from {jsonFile}.");

        //  Load the keywords
        IQueryBuilder<KeywordModel> keywordQueryBuilder = QueryBuilderFactory.CreateQueryBuilder<KeywordModel>();
        InsertAll(SchemaTableKeys.Keywords, globals.VocabTerms, keywordQueryBuilder);
        InsertAll(SchemaTableKeys.Keywords, globals.Keywords, keywordQueryBuilder);
        //  Load the regions
        IQueryBuilder<RegionModel> regionQueryBuilder = QueryBuilderFactory.CreateQueryBuilder<RegionModel>();
        InsertAll(SchemaTableKeys.Regions, globals.Regions, regionQueryBuilder);
    }

    private void LoadSet(string pJsonDir, string pSetJson)
    {
        string jsonFile = $"{pJsonDir}/{pSetJson}";
        IEnumerable<CardModel>? cards = GetModels<CardModel>(jsonFile);
        if (cards is null || !cards.Any()) {
            throw new Exception($"ERROR: Could not load the cards from {jsonFile}.");
        }

        foreach (CardModel card in cards) {
            switch (card.Type.ToUpper())
            {
                case "CHAMPION":
                    break;
                case "SPELL":
                    break;
                case "UNIT":
                    break;
                default:
                    Console.WriteLine($"{card.Type} is not supported.");
                    break;
            }
        }
    }

    private void InsertCard(CardModel pCard)
    {
        //  Load into card table
        //  Load into specific card type table
        //  Load the links
    }

    private void ResetTable(SchemaTableKeys pTableKey)
    {
        if (DbSchema.Config.TryGetValue(pTableKey, out SchemaConfig? config) || config is null)
        {
            //  No table key found.
            Console.WriteLine($"WARNING: {pTableKey} not found in the schema config. Skipping.");
            return;
        }

        _db.Drop(config.Table);
        _db.Create(config.Table, string.Join(",", config.CreateQueries));
    }

    private TModel? GetModel<TModel>(string pJsonFile) where TModel : BaseModel {
        using StreamReader reader = new(pJsonFile);
        string json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<TModel>(json);
    }

    private IEnumerable<TModel>? GetModels<TModel>(string pJsonFile) where TModel : BaseModel {
        using StreamReader reader = new(pJsonFile);
        string json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<IEnumerable<TModel>>(json);
    }

    private void InsertAll<TModel>(SchemaTableKeys pTableKey, IEnumerable<TModel> pModels, IQueryBuilder<TModel> pQueryBuilder) where TModel : BaseModel
    {
        if (DbSchema.Config.TryGetValue(pTableKey, out SchemaConfig? config) || config is null)
        {
            //  No table key found.
            Console.WriteLine($"WARNING: {pTableKey} not found in the schema config. Skipping.");
            return;
        }
        List<string> queries = [];
        foreach (TModel model in pModels) {
            queries.Add(pQueryBuilder.BuildInsertQuery(model));
        }
        _db.InsertBatch(config.Table, queries);
    }
}