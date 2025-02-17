public class CreatureActionResult
{
    public ICreature targetMaskResult;
    public bool wasSuccessful;

    public CreatureActionResult(ICreature targetMaskResult, bool wasSuccessful)
    {
        this.targetMaskResult = targetMaskResult;
        this.wasSuccessful = wasSuccessful;
    }
}
