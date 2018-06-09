using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class ChallengeProperties{

	public ChallengeProperties(){
	}
	public ChallengeProperties(int index, string name, int winAmount, int loseAmount, int unLockAmount, bool locked,int attempts){
		Name = name;
		WinAmount = winAmount;
		LoseAmount = loseAmount;
		UnLockAmount = unLockAmount;
		Locked = locked;
        Attempts = attempts;
	}
	//[XmlElement("Name")]
	public string Name{ get; set;}

	//[XmlElement("WinAmount")]
	public int WinAmount{ get; set;}

	//[XmlElement("LoseAmount")]
	public int LoseAmount{ get; set;}

	//[XmlElement("UnLockAmount")]
	public int UnLockAmount{ get; set;}

	//[XmlElement("Locked")]
	public bool Locked{ get; set;}

    public int Attempts { get; set; }

    public int Index { get; set; }
}
