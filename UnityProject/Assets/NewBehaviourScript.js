var isQuit=false;
var isMult=false;

function OnMouseEnter(){
	//change text color
	renderer.material.color=Color.blue;
}

function OnMouseExit(){
	//change text color
	renderer.material.color=Color.white;
}

function OnMouseUp(){
	//is this quit
	if (isQuit==true) {
		//quit the game
		Application.Quit();
	} else {
		if (isMult==true) {
			Application.LoadLevel("spaceShooter");
		}else{
			//load level
			Application.LoadLevel("single");
		}
	}
}

function Update(){
	//quit game if escape key is pressed
	if (Input.GetKey(KeyCode.Escape)) { Application.Quit();
	}
}