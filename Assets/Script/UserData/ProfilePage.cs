using UnityEngine;
using System.Collections;
using TMPro;

public class ProfilePage : MonoBehaviour {
	public TextField UsernameText;
	public TextField PasswordText;
	public TextField ConfirmPasswordText;
	public TextField NameText;
	public TextField SurnameText;
	public TextField TelText;
	public TextField AddressText;

	public string username {
		get {return UsernameText.text;}
		set {UsernameText.text = value;}
	}
	public string password {
		get {return PasswordText.text;}
		set {PasswordText.text = value;}
	}
	public string confirmPassword {
		get {return ConfirmPasswordText.text;}
		set {ConfirmPasswordText.text = value;}
	}
	public string name {
		get {return NameText.text;}
		set {NameText.text = value;}
	}
	public string surname {
		get {return SurnameText.text;}
		set {SurnameText.text = value;}
	}
	public string tel {
		get {return TelText.text;}
		set {TelText.text = value;}
	}
	public string address {
		get {return AddressText.text;}
		set {AddressText.text = value;}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
