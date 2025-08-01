﻿namespace LORApp.Entities.Cards;

public class UnitCardModel : CardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Unit;
    public int Attack { get; set; } = 0;
    public int Health { get; set; } = 0;
}
