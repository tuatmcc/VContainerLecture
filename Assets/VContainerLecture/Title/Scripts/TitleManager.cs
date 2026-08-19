using VContainer.Unity;
using VContainerLecture.Core.Scripts;

namespace VContainerLecture.Title.Scripts
{
    public class TitleManager : ITickable
    {
        private readonly IMenuInput menuInput;
        private readonly IGameFlowManager gameFlowManager;

        public TitleManager(IMenuInput menuInput, IGameFlowManager gameFlowManager)
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
