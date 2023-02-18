namespace Enemy
{
    public class Standard : Enemy
    {
        protected override void EarlyStart()
        {
            
        }

        protected override void AssignScriptShipValues()
        {
            SetDropValues(0);
            hp = defaultShipValues ? 22.5f : (22.5f + (Controllers.Wave.Instance.GetLevel() * 1f));
            speed = defaultShipValues ? 1 : (1f + (Controllers.Wave.Instance.GetLevel() * 0.005f));
        }

        private void Start()
        {
            flyDirection = FlyDirection.Down;
            ChangeDirection(flyDirection);
        }

        private void Update()
        {
            Fly();
        }
    }
}
