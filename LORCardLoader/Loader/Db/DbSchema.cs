namespace LORCardLoader.Loader.Db;

internal enum SchemaTableKeys {
    Keywords,
    Vocab,
    Regions,
    Cards,
    ChampionCards,
    SpellCards,
    UnitCards,
    CardAssocCardLink,
    CardKeywordLink,
    CardRegionLink,
    CardSubtypeLink
}

internal class SchemaConfig
{
    public string Table { get; set; } = string.Empty;
    public IEnumerable<string> CreateQueries { get; set; } = [];
}

internal static class DbSchema
{
    public static IReadOnlyDictionary<SchemaTableKeys, SchemaConfig> Config { get; } = new Dictionary<SchemaTableKeys, SchemaConfig>()
    {
        {
            SchemaTableKeys.Keywords, new() {
                Table = "Keywords",
                CreateQueries = [
                    @"RefCode TEXT PRIMARY KEY UNIQUE
                                   NOT NULL COLLATE NOCASE",
                    "Name TEXT NOT NULL COLLATE NOCASE",
                    "Description TEXT NOT NULL COLLATE NOCASE"
                ]
            }
        },
        {
            SchemaTableKeys.Vocab, new() {
                Table = "Vocab",
                CreateQueries = [
                    @"RefCode TEXT PRIMARY KEY UNIQUE
                                   NOT NULL COLLATE NOCASE",
                    "Name TEXT NOT NULL COLLATE NOCASE",
                    "Description TEXT NOT NULL COLLATE NOCASE"
                ]
            }
        },
        {
            SchemaTableKeys.Regions, new() {
                Table = "Regions",
                CreateQueries = [
                    @"RefCode TEXT PRIMARY KEY UNIQUE 
                                   NOT NULL COLLATE NOCASE",
                    "Name TEXT NOT NULL COLLATE NOCASE",
                    "IconPath TEXT NOT NULL COLLATE NOCASE"
                ]
            }
        },
        {
            SchemaTableKeys.Cards, new() {
                Table = "Cards",
                CreateQueries = [
                    @"CardCode TEXT PRIMARY KEY UNIQUE 
                                    NOT NULL COLLATE NOCASE",
                    "Name TEXT NOT NULL COLLATE NOCASE",
                    "CardType TEXT NOT NULL COLLATE NOCASE",
                    "CardRarity TEXT NOT NULL COLLATE NOCASE",
                    "Cost INTEGER DEFAULT (0)",
                    "ArtistName TEXT NOT NULL COLLATE NOCASE",
                    "ArtImagePath TEXT NOT NULL COLLATE NOCASE",
                    "Description TEXT NOT NULL COLLATE NOCASE",
                    "DescriptionFormatted TEXT NOT NULL COLLATE NOCASE",
                    "FlavorText TEXT NOT NULL COLLATE NOCASE",
                    "IsCollectible INTEGER DEFAULT (1)"
                ]
            }
        },
        {
            SchemaTableKeys.ChampionCards, new() {
                Table = "Champion_Cards",
                CreateQueries = [
                    @"CardCode TEXT PRIMARY KEY UNIQUE 
                                    NOT NULL COLLATE NOCASE 
                                    REFERENCES Cards (CardCode) 
                                    ON DELETE CASCADE 
                                    ON UPDATE CASCADE",
                    "Attack INTEGER DEFAULT (0)",
                    "Health INTEGER DEFAULT (0)",
                    "LevelUpDescription TEXT NOT NULL COLLATE NOCASE",
                    "LevelUpDescriptionFormatted TEXT NOT NULL COLLATE NOCASE"
                ]
            }
        },
        {
            SchemaTableKeys.SpellCards, new() {
                Table = "Spell_Cards",
                CreateQueries = [
                    @"CardCode TEXT PRIMARY KEY UNIQUE
                                    NOT NULL COLLATE NOCASE
                                    REFERENCES Cards (CardCode)
                                    ON DELETE CASCADE
                                    ON UPDATE CASCADE",
                    "SpellSpeed TEXT NOT NULL COLLATE NOCASE"
                ]
            }
        },
        {
            SchemaTableKeys.UnitCards, new() {
                Table = "Unit_Cards",
                CreateQueries = [
                    @"CardCode TEXT PRIMARY KEY UNIQUE
                                    NOT NULL COLLATE NOCASE
                                    REFERENCES Cards (CardCode)
                                    ON DELETE CASCADE
                                    ON UPDATE CASCADE",
                    "Attack INTEGER DEFAULT (0)",
                    "Health INTEGER DEFAULT (0)"
                ]
            }
        },
        {
            //  Note: Ignoring on primary key conflict as
            //  It appears that some cards have duplicate card refs in its assoc array.
            SchemaTableKeys.CardAssocCardLink, new() {
                Table = "Link_Cards_AssociatedCards",
                CreateQueries = [
                    @"CardCode TEXT NOT NULL COLLATE NOCASE
                                    REFERENCES Cards (CardCode)
                                    ON DELETE CASCADE
                                    ON UPDATE CASCADE",
                    @"AssociatedCardCode TEXT NOT NULL COLLATE NOCASE
                                              REFERENCES Cards (CardCode)
                                              ON DELETE CASCADE
                                              ON UPDATE CASCADE",
                    @"PRIMARY KEY (
                        CardCode COLLATE NOCASE,
                        AssociatedCardCode COLLATE NOCASE
                    ) ON CONFLICT IGNORE"
                ]
            }
        },
        {
            SchemaTableKeys.CardKeywordLink, new() {
                Table = "Link_Cards_Keywords",
                CreateQueries = [
                    @"CardCode TEXT NOT NULL COLLATE NOCASE
                                    REFERENCES Cards (CardCode)
                                    ON DELETE CASCADE
                                    ON UPDATE CASCADE",
                    @"RefCode TEXT NOT NULL COLLATE NOCASE
                                   REFERENCES Keywords (RefCode)
                                   ON DELETE CASCADE
                                   ON UPDATE CASCADE",
                    @"PRIMARY KEY (
                        CardCode COLLATE NOCASE,
                        RefCode COLLATE NOCASE
                    )"
                ]
            }
        },
        {
            SchemaTableKeys.CardRegionLink, new() {
                Table = "Link_Cards_Regions",
                CreateQueries = [
                    @"CardCode TEXT NOT NULL COLLATE NOCASE
                                    REFERENCES Cards (CardCode)
                                    ON DELETE CASCADE
                                    ON UPDATE CASCADE",
                    @"RefCode TEXT NOT NULL COLLATE NOCASE
                                   REFERENCES Regions (RefCode)
                                   ON DELETE CASCADE
                                   ON UPDATE CASCADE",
                    @"PRIMARY KEY (
                        CardCode COLLATE NOCASE,
                        RefCode COLLATE NOCASE
                    )"
                ]
            }
        },
        {
            SchemaTableKeys.CardSubtypeLink, new() {
                Table = "Link_Cards_Subtypes",
                CreateQueries = [
                    @"CardCode TEXT NOT NULL COLLATE NOCASE
                                    REFERENCES Cards (CardCode)
                                    ON DELETE CASCADE
                                    ON UPDATE CASCADE",
                    "Subtype TEXT NOT NULL COLLATE NOCASE",
                    @"PRIMARY KEY (
                        CardCode COLLATE NOCASE,
                        Subtype COLLATE NOCASE
                    )"
                ]
            }
        }
    };
}