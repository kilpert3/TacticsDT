using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    //step height
    public const float stepHeight = 0.25f;

    #region fields
    //track tile position and height
    public Point pos;
    public int height;

    //for placing objects on the center/top of tile
    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }

    //whats on the tile
    public GameObject content;

    //for pathfinding use (main algorithm in Board.cs)
    [HideInInspector] public Tile prev;
    [HideInInspector] public int distance;
    #endregion

    //correct tile visual after changing the height or position
    #region Private
    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }
    #endregion


    //used by board generator
    #region public
    public void Grow()
        {
            height++;
            Match();
        }

        public void Shrink()
        {
            height--;
            Match();
        }

        //for loading specific point values
        public void Load(Point p, int h)
        {
            pos = p;
            height = h;
            Match();
        }
        //overload to accept vector input
        public void Load(Vector3 v)
        {
            Load(new Point((int)v.x, (int)v.z), (int)v.y);
        }
    #endregion
}
