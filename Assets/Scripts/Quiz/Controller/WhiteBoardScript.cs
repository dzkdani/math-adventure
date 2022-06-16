using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WhiteBoardScript : MonoBehaviour
{
	public GameObject goWhiteBoard;
	public GameObject goWhiteBoardPen;
	private static LineRenderer line;
	public GameObject pen;
	private List<GameObject> list = new List<GameObject>();
	int i;

    // Update is called once per frame
    void Update()
	{
		if(goWhiteBoard.activeSelf)
        {
			if (Input.GetMouseButtonDown(0))
			{
				//clone = (GameObject)Instantiate(goWhiteBoard, goWhiteBoard.transform.position, Quaternion.identity);
				GameObject game = (GameObject)Instantiate(pen, pen.transform.position, Quaternion.identity);
				game.transform.parent = goWhiteBoardPen.transform;

				line = game.GetComponent<LineRenderer>();
				line.startColor = Color.red;
				line.startWidth = 0.2f;
				line.endWidth = 0.1f;
				i = 0;
				list.Add(game);
			}
			if (Input.GetMouseButton(0))
			{
				i++;
				line.positionCount = i;
				line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15)));
			}
		}
	}

	public void btnClear()
    {
		for(int i = 0; i < list.Count; i++)
        {
			Destroy(list[i]);
        }
	}

	public void btnOpenWhiteBoard()
    {
		inActiveWhiteBoard(true);
    }

	public void btnClose()
	{
		inActiveWhiteBoard(false);
	}

	private void inActiveWhiteBoard(bool temp)
    {
		goWhiteBoard.SetActive(temp);
		goWhiteBoardPen.SetActive(temp);
		pen.SetActive(temp);
    }

}
