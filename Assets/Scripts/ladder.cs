using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ladder : MonoBehaviour {

	// Use this for initialization
    Texture2D PlayerTexture;
	public Rect playerRect;
	public Rect[] TextureRects;

	void Start () {
		Properties.PreviousScene = "Airhockey";
        Properties.CurrentScene = "ladder";
		PlayerTexture = Properties.LoadLadderCoutries [Properties.countries];
		if (Properties.Rounds == 0) {
						
						playerRect = new Rect (84, 117, PlayerTexture.width, PlayerTexture.height);
				} else if (Properties.Rounds == 1) {
						
						playerRect = new Rect (Properties.LadderPositions [7].x, 
			                       Properties.LadderPositions [7].y, 
			                       PlayerTexture.width, 
			                       PlayerTexture.height);
				} else if (Properties.Rounds == 2) {

						playerRect = new Rect (Properties.LadderPositions [11].x, 
			                       Properties.LadderPositions [11].y, 
			                       PlayerTexture.width, 
			                       PlayerTexture.height);
				} else {

						playerRect = new Rect (Properties.LadderPositions [13].x, 
			                       Properties.LadderPositions [13].y, 
			                       PlayerTexture.width, 
			                       PlayerTexture.height);
				}

        if (Properties.TempCountries.Count == 0){
            foreach (var coutry in Properties.LoadLadderCoutries){
                if (Properties.countries != coutry.Key)
                    Properties.TempCountries.Add(coutry.Value);
            }
        }
			for (int i = 0; i < Properties.TempCountries.Count; i++){
                TextureRects[i] = new Rect(Properties.LadderPositions[i].x,
                                                    Properties.LadderPositions[i].y,
                                                            Properties.TempCountries[i].width,
                                                                  Properties.TempCountries[i].height);
            }   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
		if (Properties.TempCountries.Count != 0 && Properties.Rounds == 0) {
						for (int i = 0; i < Properties.TempCountries.Count; i++) {
								GUI.DrawTexture (TextureRects [i], Properties.TempCountries [i]);
						}
			GUI.DrawTexture(playerRect,PlayerTexture);
				}
		if (Properties.Rounds == 1 || Properties.Rounds==2) {
			GUI.DrawTexture(new Rect(Properties.LadderPositions[8].x,
			                         Properties.LadderPositions[8].y,
			                         Properties.TempCountries [Properties.semis_1].width,
			                         Properties.TempCountries [Properties.semis_1].height), 
			                Properties.TempCountries [Properties.semis_1]);
			GUI.DrawTexture(new Rect(Properties.LadderPositions[9].x,
			                         Properties.LadderPositions[9].y,
			                         Properties.TempCountries [Properties.semis_2].width,
			                         Properties.TempCountries [Properties.semis_2].height), 
			                Properties.TempCountries [Properties.semis_2]);
			GUI.DrawTexture(new Rect(Properties.LadderPositions[10].x,
			                         Properties.LadderPositions[10].y,
			                         Properties.TempCountries [Properties.semis_3].width,
			                         Properties.TempCountries [Properties.semis_3].height), 
			                Properties.TempCountries [Properties.semis_3]);
			GUI.DrawTexture(playerRect,PlayerTexture);
				}
		if (Properties.Rounds == 2||Properties.Rounds==3) {
			GUI.DrawTexture(playerRect,PlayerTexture);
			GUI.DrawTexture(new Rect(Properties.LadderPositions[12].x,
			                         Properties.LadderPositions[12].y,
			                         Properties.TempCountries [Properties.finals_1].width,
			                         Properties.TempCountries [Properties.finals_1].height), 
			                Properties.TempCountries [Properties.finals_1]);
				}
		if (Properties.Rounds == 3) {
			GUI.DrawTexture(playerRect,PlayerTexture);
				}
    }

    IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
    {
        List<int> candidates = new List<int>();
        for (int i = minInclusive; i <= maxInclusive; i++){
            candidates.Add(i);
        }
        while (candidates.Count > 0){
            int index = Random.Range(0,candidates.Count);
            yield return candidates[index];
            candidates.RemoveAt(index);
        }
    }
}
