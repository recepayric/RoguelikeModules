using Runtime.Model;
using MVC.Base.Runtime.Abstract.Model;
using Runtime.Data.ValueObject;
using strange.extensions.command.impl;

namespace Runtime.Controller
{
    public class InitPoolCommand : Command
    {
        [Inject] public IObjectPoolModel PoolModel { get; set; }
        [Inject] public IGameModel GameModel { get; set; }
        public override void Execute()
        {
            for (int i = 0; i < GameModel.Pool.List.Count; i++)
            {
                PoolVO item = GameModel.Pool.List[i];
                PoolModel.Pool(item.Key.ToString(),item.Prefab,item.Count);
            }
        }
    }
}