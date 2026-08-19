using VContainer.Unity;
using VContainerLecture.Core.Scripts;

namespace VContainerLecture.Result.Scripts
{
    public class ResultManager : ITickable
    {
        private readonly IMenuInput menuInput;
        private readonly IGameFlowManager gameFlowManager;

        public ResultManager(IMenuInput menuInput, IGameFlowManager gameFlowManager)
        {
            this.menuInput = menuInput;
            this.gameFlowManager = gameFlowManager;
        }

        public void Tick()
        {
            if (menuInput.SubmitPressed)
            {
                gameFlowManager.NextState(TransitionType.Enter);
            }
        }
    }
}
