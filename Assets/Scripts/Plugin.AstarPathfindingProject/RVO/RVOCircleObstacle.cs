using UnityEngine;
using System.Collections;
using Pathfinding.RVO;
using Pathfinding.RVO.Sampled;

namespace Pathfinding.RVO
{

    /**
     * Circle Obstacle for RVO Simulation.
     * 
     * \astarpro 
     */
    [AddComponentMenu("Pathfinding/Local Avoidance/Circle Obstacle")]
    public class RVOCircleObstacle : RVOObstacle
    { 
        public float height = 1;

        public float radius = 1;

        public int circleResolution = 6;

        protected override bool StaticObstacle { get { return false; } }
        protected override bool ExecuteInEditor { get { return true; } }
        protected override bool LocalCoordinates { get { return true; } }
        protected override float Height { get { return height; } }

        private int _radius;
        private int _circleResolution = 6;
        private int _height;


        protected override bool AreGizmosDirty()
        {
            radius = Mathf.Abs(radius);
            circleResolution = Mathf.Min(Mathf.Max(Mathf.Abs(circleResolution), 6), 12);
            height = Mathf.Abs(height);

            bool ret = _radius != IntMath.Scale2Int(radius) || _circleResolution != circleResolution || _height != IntMath.Scale2Int(height);

            _radius = IntMath.Scale2Int(radius);
            _height = IntMath.Scale2Int(height);
            _circleResolution = circleResolution;

            return ret;
        }

        protected override void CreateObstacles()
        {
        }
    }
}