using UnityEngine;
using System.Collections;

public class CreateResultList : MonoBehaviour {
	public GameObject rList;
	public ResultLayer rLayer;

	public bool stateClick = true;

	private int number=0;
	private float spaceBox = 0.82f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void createResultList(CheckDownloader data, string ProductType){
		this.gameObject.transform.localPosition = new Vector3 (-1f,3.65f,0);

		if(ProductType=="1"){ // Cosmetic
			if(data.contentList.Length>1){//create List
				for(int i=0;i<data.contentList.Length;i++){
					GameObject itemDetail = (GameObject)GameObject.Instantiate(rList);
					itemDetail.SetActive(true);
					itemDetail.transform.parent = this.transform;
					itemDetail.transform.localPosition = new Vector3 (1f, 0f - i * spaceBox, 0);
					
					ResultList idt = itemDetail.GetComponent<ResultList>();
					idt.setup( data, i, rLayer.selectListCallback, ProductType );
					rLayer.resultList.Add(itemDetail);
				}
			}
			number = data.contentList.Length;
		}
		else if(ProductType=="2.1"){ // Danger Register
			if(data.contentListDangerRegister.Length>1){//create List
				for(int i=0;i<data.contentListDangerRegister.Length;i++){
					GameObject itemDetail = (GameObject)GameObject.Instantiate(rList);
					itemDetail.SetActive(true);
					itemDetail.transform.parent = this.transform;
					itemDetail.transform.localPosition = new Vector3 (1f, 0f - i * spaceBox, 0);
					
					ResultList idt = itemDetail.GetComponent<ResultList>();
					idt.setup( data, i, rLayer.selectListCallback, ProductType );
					rLayer.resultList.Add(itemDetail);
				}
			}
			number = data.contentListDangerRegister.Length;
		}
		else if(ProductType=="2.2"){ // Danger License
			Debug.Log ("Data Search Length : "+data.contentListDangerLicense.Length);
			if(data.contentListDangerLicense.Length>1){//create List
				for(int i=0;i<data.contentListDangerLicense.Length;i++){
					GameObject itemDetail = (GameObject)GameObject.Instantiate(rList);
					itemDetail.SetActive(true);
					itemDetail.transform.parent = this.transform;
					itemDetail.transform.localPosition = new Vector3 (1f, 0f - i * spaceBox, 0);
					
					ResultList idt = itemDetail.GetComponent<ResultList>();
					idt.setup( data, i, rLayer.selectListCallback, ProductType );
					rLayer.resultList.Add(itemDetail);
				}
			}
			number = data.contentListDangerLicense.Length;
		}
		else if(ProductType=="3"){ // Drug
			if(data.contentListDrug.Length>1){//create List
				for(int i=0;i<data.contentListDrug.Length;i++){
					GameObject itemDetail = (GameObject)GameObject.Instantiate(rList);
					itemDetail.SetActive(true);
					itemDetail.transform.parent = this.transform;
					itemDetail.transform.localPosition = new Vector3 (1f, 0f - i * spaceBox, 0);
					
					ResultList idt = itemDetail.GetComponent<ResultList>();
					idt.setup( data, i, rLayer.selectListCallback, ProductType );
					rLayer.resultList.Add(itemDetail);
				}
			}
			number = data.contentListDrug.Length;
		}
		else if(ProductType=="4"){ // Food
			if(data.contentListFood.Length>1){//create List
				for(int i=0;i<data.contentListFood.Length;i++){
					GameObject itemDetail = (GameObject)GameObject.Instantiate(rList);
					itemDetail.SetActive(true);
					itemDetail.transform.parent = this.transform;
					itemDetail.transform.localPosition = new Vector3 (1f, 0f - i * spaceBox, 0);
					
					ResultList idt = itemDetail.GetComponent<ResultList>();
					idt.setup( data, i, rLayer.selectListCallback, ProductType );
					rLayer.resultList.Add(itemDetail);
				}
			}
			number = data.contentListFood.Length;
		}
		else if(ProductType=="5"){ // Tool
			if(data.contentListTool.Length>1){//create List
				for(int i=0;i<data.contentListTool.Length;i++){
					GameObject itemDetail = (GameObject)GameObject.Instantiate(rList);
					itemDetail.SetActive(true);
					itemDetail.transform.parent = this.transform;
					itemDetail.transform.localPosition = new Vector3 (1f, 0f - i * spaceBox, 0);

					ResultList idt = itemDetail.GetComponent<ResultList>();
					idt.setup( data, i, rLayer.selectListCallback, ProductType );
					rLayer.resultList.Add(itemDetail);
				}
			}
			number = data.contentListTool.Length;
		}
		rLayer.setScrollable ((number * spaceBox));
//		Debug.Log (this.transform.localScale.y);
	}
}
