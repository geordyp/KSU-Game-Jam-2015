using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class GameSceneControllerScript : MonoBehaviour {

	/**************************
	 * Variables
	 **************************/
	//Password Variables
	public string password = "";
	public int strength = 0;
	public bool passwordSet = false;
	private Dictionary<string, bool> _pwDictionary;
	public bool needNewPassword = false;

	//Binary Variables
	private char _currentBinaryEnemy;

	//Game Variables
	private AudioSource _audioSource;

	/**************************
	 * GameObjects
	 **************************/
	//Desktop Objects
	public GameObject[] filesToSave;

	//Taskbar Items
	public GameObject menuButtonFinder;
	public GameObject menuButtonPassword;
	public GameObject menuButtonChrome;
	public GameObject menuButtonDesktop;

	//Game Functions
	public GameObject restartButton;

	//Password Objects
	public GameObject inputFieldPassword;
	public GameObject passwordWindow;
	public GameObject passwordStatusText;
	public GameObject passwordTimer;
	public GameObject passwordButton;

	//Browser Objects
	public GameObject browserWindow;
	public GameObject binaryText1;
	public GameObject binaryText2;
	public GameObject binaryText3;
	public GameObject binaryText4;
	public GameObject binaryText5;
	public GameObject binaryText6;
	public GameObject binaryText7;
	public GameObject binaryText8;

	//Finder Objects
	public GameObject gameWindow;
	public GameObject gameFolder;
	public GameObject gameText;
	public GameObject gameRestoreButton;

	//Misc
	public GameObject documentsFolder;
	private Quaternion _defaultRotation;
	public float count = 0;
	private bool _desktopIsVisible;
	public bool gameInProgress = false;
	public bool gameInProgressP = true;	//Exclusive to password
	private int _gameClock = 300;
	public GameObject gameClockText;
	public GameObject gameOverMusic;
	public GameObject bugIcon;

	//Intialization
	void Start ()
	{
		PlayMusic ();

		_pwDictionary = new Dictionary<string, bool> ();
		inputFieldPassword = GameObject.Find("PasswordInputField");
		passwordWindow = GameObject.Find ("PasswordWindow");
		passwordStatusText = GameObject.Find ("PasswordStatus");
		passwordTimer = GameObject.Find ("PasswordTimer");
		passwordButton = GameObject.Find ("PasswordButton");
		passwordStatusText.GetComponent<Text>().text = "Enter your password and press ENTER.";
		passwordTimer.GetComponent<Text>().text = "";

		restartButton = GameObject.Find ("RestartButton");
		restartButton.SetActive (false);

		menuButtonFinder = GameObject.Find ("SwitchToDesktopButton");
		menuButtonPassword = GameObject.Find ("SwitchToPasswordButton");
		menuButtonChrome = GameObject.Find ("SwitchToBrowserButton");;
		menuButtonDesktop = GameObject.Find ("SwitchToDesktopIconButton");;

		browserWindow = GameObject.Find ("BrowserWindow");

		binaryText1 = GameObject.Find ("BinaryTextBox1");
		binaryText2 = GameObject.Find ("BinaryTextBox2");
		binaryText3 = GameObject.Find ("BinaryTextBox3");
		binaryText4 = GameObject.Find ("BinaryTextBox4");
		binaryText5 = GameObject.Find ("BinaryTextBox5");
		binaryText6 = GameObject.Find ("BinaryTextBox6");
		binaryText7 = GameObject.Find ("BinaryTextBox7");
		binaryText8 = GameObject.Find ("BinaryTextBox8");

		documentsFolder = GameObject.Find ("DocumentsFolder");

		gameWindow = GameObject.Find ("ComputerGameWindow");
		gameFolder = GameObject.Find("DownloadsFolder");
		gameText = GameObject.Find("ComputerGameTextBox");
		gameRestoreButton = GameObject.Find("GameRestoreButton");

		gameClockText = GameObject.Find ("GameClockTextBox");

		bugIcon = GameObject.Find ("BugIcon");

		SwitchToPasswordWindow ();
		passwordButton.SetActive (false);

		_defaultRotation = documentsFolder.transform.rotation;

		_currentBinaryEnemy = binaryText1.GetComponent<Text> ().text [binaryText1.GetComponent<Text>().text.Length - 1];
	}

	void PlayMusic()
	{
		_audioSource = gameObject.AddComponent<AudioSource> ();
		//_audioSource.clip = Resources.Load ("MainMusic") as AudioClip;
		_audioSource.clip = Resources.Load ("GameMusic") as AudioClip;
		_audioSource.loop = true;
		_audioSource.Play ();
	}
	
	// Wait coroutine.
	IEnumerator UpdateBinaryEnemies() {
		int r = Random.Range (0, 100);
		if (r <= 50) {
			binaryText1.GetComponent<Text> ().text += "1";
			_currentBinaryEnemy = binaryText1.GetComponent<Text> ().text [binaryText1.GetComponent<Text> ().text.Length - 1];
		} else {
			binaryText1.GetComponent<Text> ().text += "0";
			_currentBinaryEnemy = binaryText1.GetComponent<Text> ().text [binaryText1.GetComponent<Text> ().text.Length - 1];
		}

		yield return new WaitForSeconds (3);

		StartCoroutine (UpdateBinaryEnemies ());
	}

// Wait coroutine for restoring files game.
	IEnumerator UpdateRestoreFile()
	{
		if (gameText.GetComponent<Text> ().text.Equals ("")) {
			GameOverScene (1);
		}

		gameText.GetComponent<Text> ().text = gameText.GetComponent<Text> ().text.Substring (0, gameText.GetComponent<Text> ().text.Length - 1);
		yield return new WaitForSeconds (1);
	
		StartCoroutine (UpdateRestoreFile ());
	}

	// Game clock.
	IEnumerator GameClock()
	{
		if(_gameClock == 0)
		{
			GameOverScene(0);
		}
		else
		{
			_gameClock--;
			bugIcon.transform.position = new Vector3(bugIcon.transform.position.x, bugIcon.transform.position.y - 0.017f, bugIcon.transform.position.z);

			yield return new WaitForSeconds (1);
			
			StartCoroutine (GameClock ());
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (gameInProgress) {
	
			if (binaryText1.GetComponent<Text> ().text.Length >= 86) {
				GameOverScene(3);
			}

			binaryText2.GetComponent<Text> ().text = binaryText1.GetComponent<Text> ().text;
			binaryText3.GetComponent<Text> ().text = binaryText1.GetComponent<Text> ().text;
			binaryText4.GetComponent<Text> ().text = binaryText1.GetComponent<Text> ().text;
			binaryText5.GetComponent<Text> ().text = binaryText1.GetComponent<Text> ().text;
			binaryText6.GetComponent<Text> ().text = binaryText1.GetComponent<Text> ().text;
			binaryText7.GetComponent<Text> ().text = binaryText1.GetComponent<Text> ().text;
			binaryText8.GetComponent<Text> ().text = binaryText1.GetComponent<Text> ().text;

			if(binaryText1.GetComponent<Text> ().enabled)
			{
				if(Input.GetKeyDown(KeyCode.Alpha0) && _currentBinaryEnemy.Equals('0'))
				{
					binaryText1.GetComponent<Text>().text = binaryText1.GetComponent<Text>().text.Substring (0, binaryText1.GetComponent<Text>().text.Length - 1);
					_currentBinaryEnemy = binaryText1.GetComponent<Text> ().text [binaryText1.GetComponent<Text>().text.Length - 1];
				}
				else if(Input.GetKeyDown(KeyCode.Alpha0) && _currentBinaryEnemy.Equals('1'))
				{
					binaryText1.GetComponent<Text>().text += "01010";
					_currentBinaryEnemy = binaryText1.GetComponent<Text> ().text [binaryText1.GetComponent<Text>().text.Length - 1];
				}
				else if(Input.GetKeyDown(KeyCode.Alpha1) && _currentBinaryEnemy.Equals('1'))
				{
					binaryText1.GetComponent<Text>().text = binaryText1.GetComponent<Text>().text.Substring (0, binaryText1.GetComponent<Text>().text.Length - 1);
					_currentBinaryEnemy = binaryText1.GetComponent<Text> ().text [binaryText1.GetComponent<Text>().text.Length -1 ];
				}
				else if(Input.GetKeyDown(KeyCode.Alpha1) && _currentBinaryEnemy.Equals('0'))
				{
					binaryText1.GetComponent<Text>().text += "10101";
					_currentBinaryEnemy = binaryText1.GetComponent<Text> ().text [binaryText1.GetComponent<Text>().text.Length -1 ];
				}
			}

			// ROTATION OF MAIN FOLDER ON DESKTOP.
			if (_desktopIsVisible && Input.GetKey (KeyCode.Space)) {
				if (documentsFolder.transform.rotation.eulerAngles.z >= 170) {
					Instantiate (filesToSave [Random.Range (0, filesToSave.Length)]);
				}
				else {
					documentsFolder.transform.Rotate (0, 0, 10);	
				}
			}
			else if (_desktopIsVisible) {
				if (documentsFolder.transform.rotation.eulerAngles.z <= 0) {
				}
				else {
					documentsFolder.transform.Rotate (0, 0, -10);
				}
				}

			gameClockText.GetComponent<Text>().text = _gameClock.ToString() + " second(s) left";
		}
	}

	public void PasswordWasEntered()
	{
		if (!passwordSet){

			//Get string user typed
			password = inputFieldPassword.GetComponent<InputField>().text;

			if (CheckPassword (password)) {

				CancelInvoke();

				//Say good input
				passwordStatusText.GetComponent<Text>().text = "Password confirmed.";

				passwordButton.SetActive (true);
				needNewPassword = true;

				//GAME HAS BEGUN
				gameInProgress = true;
				StartCoroutine (UpdateBinaryEnemies());
				StartCoroutine (UpdateRestoreFile ());
				StartCoroutine (GameClock());

				//Disable text box
				//Can only be re-enabled through "New Password" button
				inputFieldPassword.SetActive(false);

				//Initialize strength
				strength = 0;
				//get the strength value
				strength = (GetPasswordStrength(password) * GetPasswordStrength(password));

				if(strength >= 250)
				{
					strength = 250;
				}

				passwordTimer.GetComponent<Text>().text = (strength).ToString();

				//Allow for new password to be entered
				passwordButton.SetActive (true);
				needNewPassword = true;

				passwordSet = true;

				InvokeRepeating("UpdatePasswordTimer", 1.0f, 1.0f);
			}
			else {
				passwordSet = false;
			}
		}
		else {
			if ((inputFieldPassword.GetComponent<InputField>().text).Equals(password)) {
				//Say correct and enter new password
				passwordStatusText.GetComponent<Text>().text = "Correct. Enter your new password and press ENTER.";
				passwordSet = false;
			}
			else {
				//Say incorrect password and re-enter
				passwordStatusText.GetComponent<Text>().text = "Incorrect password. Re-enter your current password and press ENTER.";
				passwordSet = true;
			}
		}

		inputFieldPassword.GetComponent<InputField> ().text = "";
	}

	public void NewPasswordButton(){
		inputFieldPassword.SetActive(true);

		passwordButton.SetActive (false);
		needNewPassword = false;
	
		passwordStatusText.GetComponent<Text> ().text = "Enter your CURRENT password and press ENTER.";
	}

	void UpdatePasswordTimer()
	{	
		if(strength == 0)
		{
			GameOverScene(2);
			CancelInvoke();
		}
		else
		{
			strength--;
			
			passwordTimer.GetComponent<Text>().text = ("It will take " + strength + " seconds for the virus to learn your current password.");
		}
	}

	// Switch to password window.
	public void SwitchToPasswordWindow()
	{
		if (gameInProgressP) {
			passwordWindow.renderer.sortingOrder = 1;
			passwordStatusText.SetActive (true);
			if (needNewPassword) {
				passwordButton.SetActive (true);
				inputFieldPassword.SetActive (false);
			}
			else {
				passwordButton.SetActive(false);
				inputFieldPassword.SetActive (true);
			}

			passwordTimer.SetActive(true);

			browserWindow.renderer.sortingOrder = 0;

			documentsFolder.renderer.sortingOrder = 0;

			gameWindow.renderer.sortingOrder = 0;
			gameFolder.renderer.sortingOrder = 0;
			gameText.SetActive (false);
			gameRestoreButton.SetActive (false);

			binaryText1.GetComponent<Text> ().enabled = false;
			binaryText2.GetComponent<Text> ().enabled = false;
			binaryText3.GetComponent<Text> ().enabled = false;
			binaryText4.GetComponent<Text> ().enabled = false;
			binaryText5.GetComponent<Text> ().enabled = false;
			binaryText6.GetComponent<Text> ().enabled = false;
			binaryText7.GetComponent<Text> ().enabled = false;
			binaryText8.GetComponent<Text> ().enabled = false;

			_desktopIsVisible = false;
		}
	}

	// Browser.
	public void SwitchToBrowserWindow()
	{
		if (gameInProgress) {
			inputFieldPassword.SetActive (false);
			passwordWindow.renderer.sortingOrder = -1;
			passwordStatusText.SetActive (false);
			passwordButton.SetActive (false);

			passwordTimer.SetActive(false);
		
			browserWindow.renderer.sortingOrder = 1;

			documentsFolder.renderer.sortingOrder = 0;

			gameWindow.renderer.sortingOrder = 0;
			gameFolder.renderer.sortingOrder = 0;
			gameText.SetActive (false);
			gameRestoreButton.SetActive (false);

			binaryText1.GetComponent<Text> ().enabled = true;
			binaryText2.GetComponent<Text> ().enabled = true;
			binaryText3.GetComponent<Text> ().enabled = true;
			binaryText4.GetComponent<Text> ().enabled = true;
			binaryText5.GetComponent<Text> ().enabled = true;
			binaryText6.GetComponent<Text> ().enabled = true;
			binaryText7.GetComponent<Text> ().enabled = true;
			binaryText8.GetComponent<Text> ().enabled = true;

			_desktopIsVisible = false;
		}
	}

	// Switch to desktop window.
	public void SwitchToDesktopWindow()
	{
		if (gameInProgress) {
			inputFieldPassword.SetActive (false);
			passwordWindow.renderer.sortingOrder = -1;
			passwordStatusText.SetActive (false);
			passwordButton.SetActive (false);
		
			passwordTimer.SetActive(false);

			browserWindow.renderer.sortingOrder = 0;

			documentsFolder.renderer.sortingOrder = 1;

			gameWindow.renderer.sortingOrder = -1;
			gameFolder.renderer.sortingOrder = 0;
			gameText.SetActive (false);
			gameRestoreButton.SetActive (false);

			binaryText1.GetComponent<Text> ().enabled = false;
			binaryText2.GetComponent<Text> ().enabled = false;
			binaryText3.GetComponent<Text> ().enabled = false;
			binaryText4.GetComponent<Text> ().enabled = false;
			binaryText5.GetComponent<Text> ().enabled = false;
			binaryText6.GetComponent<Text> ().enabled = false;
			binaryText7.GetComponent<Text> ().enabled = false;
			binaryText8.GetComponent<Text> ().enabled = false;

			_desktopIsVisible = true;
		}
	}

	// Switch to game window.
	public void SwitchToGameWindow()
	{
		if (gameInProgress) {
			inputFieldPassword.SetActive (false);
			passwordWindow.renderer.sortingOrder = -1;
			passwordStatusText.SetActive (false);
			passwordButton.SetActive (false);
		
			passwordTimer.SetActive(false);

			browserWindow.renderer.sortingOrder = 0;

			documentsFolder.renderer.sortingOrder = 0;

			gameWindow.renderer.sortingOrder = 1;
			gameFolder.renderer.sortingOrder = 2;
			gameText.SetActive (true);
			gameRestoreButton.SetActive (true);

			binaryText1.GetComponent<Text> ().enabled = false;
			binaryText2.GetComponent<Text> ().enabled = false;
			binaryText3.GetComponent<Text> ().enabled = false;
			binaryText4.GetComponent<Text> ().enabled = false;
			binaryText5.GetComponent<Text> ().enabled = false;
			binaryText6.GetComponent<Text> ().enabled = false;
			binaryText7.GetComponent<Text> ().enabled = false;
			binaryText8.GetComponent<Text> ().enabled = false;

			_desktopIsVisible = false;
		}
	}

	public bool CheckPassword(string p) {
		if (p.Length <= 1) {
			passwordStatusText.GetComponent<Text>().text = "Password must be at least 2 characters.";
			return false;
		}
		if(_pwDictionary.ContainsKey(p)) {
			passwordStatusText.GetComponent<Text>().text = "You've used this password.";
			return false;
		}
		else {
			_pwDictionary.Add(p, true);
			return true;
		}
	}

	/* Sets the password strength which will
	 * be used to calculate the amount of time
	 * it takes to crack the password.
	 * Strong password = long time
	 */
	public int GetPasswordStrength(string p) {
		//Add strength based on length
		int s = 0;
		s += password.Length;
		
		//special conditions
		bool hasUpperCase = false;
		bool hasLowerCase = false;
		bool hasNumber = false;
		bool hasSpecialChar = false;
		char currentChar;

		//Add strength based on
		for (int i = 0; i < password.Length; i++) {
			currentChar = password[i];
			
			if (currentChar >= 65 && currentChar <= 90) {
				//has an upper case letter
				hasUpperCase = true;
			}
			else if (currentChar >= 97 && currentChar <= 122) {
				//has a lower case letter
				hasLowerCase = true;
			}
			else if (currentChar >= 49 && currentChar <= 57) {
				//has a number
				hasNumber = true;
			}
			else if (currentChar == 33 || currentChar == 35 || currentChar == 36 || currentChar == 38) {
				//has a special character
				hasSpecialChar = true;
			}
			else {
				//do nothing
			}
		}
		
		//increase strength based on having the special conditions
		if (hasUpperCase) s++;
		if (hasLowerCase) s++;
		if (hasNumber) s++;
		if (hasSpecialChar) s++;
		
		return s;
	}

	public void RestoreGameButton()
	{
		gameText.GetComponent<Text> ().text = "NOT_WHERE_I_KEEP_MY_IMPORTANT_PASSWORDS.txt";
	}

	public void GameOverScene(int i)
	{
		gameInProgress = false;
		gameInProgressP = false; //Exclusive to password

		inputFieldPassword.SetActive (false);
		passwordWindow.renderer.sortingOrder = -1;
		passwordStatusText.SetActive (false);
		passwordButton.SetActive (false);
		passwordTimer.SetActive (false);
		
		browserWindow.renderer.sortingOrder = 0;
		
		documentsFolder.renderer.sortingOrder = 0;
		
		gameWindow.renderer.sortingOrder = -1;
		gameFolder.renderer.sortingOrder = 0;
		gameText.SetActive (false);
		gameRestoreButton.SetActive (false);
		
		binaryText1.GetComponent<Text> ().enabled = false;
		binaryText2.GetComponent<Text> ().enabled = false;
		binaryText3.GetComponent<Text> ().enabled = false;
		binaryText4.GetComponent<Text> ().enabled = false;
		binaryText5.GetComponent<Text> ().enabled = false;
		binaryText6.GetComponent<Text> ().enabled = false;
		binaryText7.GetComponent<Text> ().enabled = false;
		binaryText8.GetComponent<Text> ().enabled = false;
		
		_desktopIsVisible = false;

		menuButtonFinder.SetActive (false);
		menuButtonPassword.SetActive (false);
		menuButtonChrome.SetActive (false);
		menuButtonDesktop.SetActive (false);

		StopCoroutine (GameClock ());

		bugIcon.SetActive (false);

		restartButton.SetActive (true);

		if (i == 0) {
			menuButtonDesktop.SetActive (true);
		} else if (i == 1) {
			menuButtonFinder.SetActive (true);
		} else if (i == 2) {
			menuButtonPassword.SetActive (true);
		} else if (i == 3) {
			menuButtonChrome.SetActive (true);
		}
	}

	public void ShutDown()
	{
		Application.Quit ();
	}

	public void RestartGame()
	{
		Application.LoadLevel ("GameScene");
	}
}