public enum Debuffs
{
    Stunned,  //stunned shapes cant move
    Confused, //confused shapes move backwards slower tho
    Slowed,   //slowed shapes are... slow
    Fire,     //shapes that are on fire cause other shapes to be on fire by chance fire goes away after a few seconds, does like 1 damage per half second or something
    Worn,     ///Worn shapes take the damage multiplier defined in <see cref="GameUtils"/>
}
