using UnityEngine;

public class RumtimeFollower : MonoBehaviour
{
    public OptionalGridMenager grid;
    bool attackState = false;
    bool active = false;
    private void Update()
    {
        if (active)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (attackState) grid.AttackFollow(hit.point);
                else grid.Follow(hit.point);
            }

            if (Input.GetMouseButtonDown(1)) 
            {
                grid.Follow(Vector3.left * 10);
                grid.AttackFollow(Vector3.left * 10); 
                attackState = !attackState; 
            }
            if (Input.GetMouseButtonDown(0)) 
            { 
                grid.Follow(Vector3.left * 10);
                grid.AttackFollow(Vector3.left * 10);
                active = false; 
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) { active = true; }
        }
    }
}
