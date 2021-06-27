

public enum FormationRank
{
    Leader,
    Soldier,
    None
}



public enum HatsTypes
{
    None,
    CowBoy,
    Crown,
    Magician,
    Miner,
    Mustache,
    Pajama,
    Pillbox,
    Police,
    Shower,
    Sombrero,
    Viking


}

#region States 
public enum CAction
{
    None,
    GoToTarget,
    GoingToEnemy,
    AttackEnemy,
    Retreat,
    Defend,
    Forming

}

public enum State
{
    Normal,
    Waiting,
    Action,
    Ended,
    
}

#endregion