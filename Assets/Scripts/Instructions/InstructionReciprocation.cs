namespace PipelineDreams
{
    public class InstructionReciprocation : Instruction {

        public override IClockTask Operation(float startClock)
        {

            return PassParam(new InstructionReciprocationTask());
        }
        
    }
    public abstract partial class Instruction
    {
        /// <summary>
        /// Field instruction task used above.
        /// </summary>
        protected class InstructionReciprocationTask : InstructionTask
        {
            protected override void OnRunStart()
            {
            }
        }
    }

}