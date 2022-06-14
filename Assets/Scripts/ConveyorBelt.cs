using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 1;
    
    public ConveyorBelt next;
    
    [Space(10)]
    public List<Transform> points;
    
    [Space(10)]
    public List<ConveyorItem> items;
    
    private ConveyorPath path;
    
    private float backupDist;
    
    // Start is called before the first frame update
    private void Start()
    {
        path = new ConveyorPath(points);
        for(int i = 0; i < items.Count; i++)
        {
            float distance = 0.5f * i;
            items[i].totalDist = distance;
            
            Vector3 point = path.GetPointAtDistance(distance);
            items[i].transform.position = point;
        }
    }
    
    // Update is called every frame update
    private void Update()
    {
        for(int i = 0; i < items.Count; i++)
        {
            UpdateItem(items[i]);
        }
    }
    
    public void TakeItem(ConveyorItem item)
    {
        items.Add(item);
    }
    
    private void UpdateItem(ConveyorItem item)
    {
        if(!item.isWaiting)
        {     
            item.totalDist += Time.deltaTime * speed;
            
            float maxDist = path.length - backupDist;
            
            if(item.totalDist >= maxDist)
            {
                if(next != null)
                {
                    next.TakeItem(item);
                }
                else 
                {
                    backupDist += 0.5f;
                    item.isWaiting = true;
                }
                
                item.totalDist = maxDist;
            }
            
            item.transform.position = path.GetPointAtDistance(item.totalDist);
        }
    }
    
    //Inverse Lerp between vector3 on line
    private float InverseLerp(Vector3 target, Vector3 last, Vector3 position)
    {
        //target - C, position - B, last - A
        float CB = (target - position).magnitude;
        float CA = (target - last).magnitude;
        return CB / CA;
    }
}
