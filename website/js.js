function loadWebpage()
{
	if(document.getElementsByTagName("footer").length>0)
		document.getElementsByTagName("footer")[0].innerHTML = loadFooter();
	if(document.getElementById("profilePageMainForm")!=null) profilePageMainForm();
	if(document.getElementById("frontPageTopNav")!=null) topNavPage();
}

function loadFooter()
{
	return '<span class="boxSpan">Created by Nejc Smrkolj Kozelj</span><span class="boxSpan">Contact information: <a href="mailto:nejcsk@hotmail.com">nejcsk@hotmail.com</a></span>';
}

function landingPageLoginFormLoad()
{
	return '<form name="landingPageLoginForm" action="frontPage.html">	<span>		Username:	</span>	<span>		<input type="text" name="username" placeholder="Enter username" required />	</span>	<span>		Password:	</span>	<span>		<input type="password" name="password" placeholder="Enter password" required/>	</span>	<span class="landingPageChangeLoginForm" id="landingPageChangeLoginFormForgotData" onclick="landingPageChangeToForgotLoginData()">		Forgot username/password?	</span>	<span>		<input type="submit" name="login" value="Login"/>	</span></form>';
}

function topNavPageLoad()
{
	return '<section id="frontPageTopNavLeft"><article><a href="frontPage.html" class="none"><img src="Resources/logo.jpg" id="frontPageLogoImage" /></a><a href="record.html" class="none"><img src="Resources/record.jpg" class="frontPageSmallImage" /></a></article></section><section id="frontPageTopNavRight"><article><!--<a href="landing.html" class="none">--><a href="landing.html" class="none"><img src="Resources/exit.png" class="frontPageSmallImage" />Logout</a></article><article><a href="profilePage.html" class="none"><img src="Resources/avatar.png" class="frontPageSmallImage" />Open profile</a></article></section>';
	
}

function landingPageRegisterFormLoad()
{
	return '<form name="landingPageRegisterForm" action="frontPage.html">	<span>		Username:	</span>	<span>		<input type="text" name="username" placeholder="Enter username" required/>	</span>	<span>		Password:	</span>	<span>		<input type="password" name="password" placeholder="Enter password" required/>	</span>		<span>		Repeat password:	</span>	<span>		<input type="password" name="passwordRepeat" placeholder="Repeat password" required/>	</span>		<span>		Email:	</span>	<span>		<input type="email" name="email" placeholder="Enter email" required/>	</span>		<span>		Repeat email:	</span>	<span>		<input type="email" name="emailRepeat" placeholder="Repeat email" required/>	</span>	<span>		Display name:	</span>	<span>		<input type="text" name="displayName" placeholder="Display name" required/>	</span>	<span>	</span>	<span>		<input type="submit" name="register" value="Register"/>	</span></form>';
}

function landingPageForgotLoginFormLoad()
{
	return '<form name="landingPageForgotLoginForm" action="frontPage.html">	<span>		Enter email:	</span>	<span>		<input type="email" name="email" placeholder="Enter email" required />	</span>	<span>	</span>	<span>		<input type="submit" name="forgotLogin" value="Recover"/>	</span></form>';
	
}
function profilePageMainFormLoad()
{
	return '<form name="profilePageChangeSettings" action="profilePage.html"><span>Old email:</span> <span><input type="email" name="oldEmail" placeholder="Enter old email"/></span> <span>New email:</span> <span><input type="email" name="newEmail" placeholder="Enter new email"/></span> <span>Repeat new email:</span> <span><input type="email" name="newEmailRepeat" placeholder="Repeat new email"/></span> <span>&nbsp </span><span class="profilePageSubmitButton"><input type="submit" name="changeEmail" value="Change email"/></span> <span>&nbsp </span><span>&nbsp </span><span>Old password:</span> <span><input type="password" name="oldPassword" placeholder="Enter old password"/></span> <span>New password:</span> <span><input type="password" name="newPassword" placeholder="Enter new password"/></span> <span>Repeat new password:</span> <span><input type="password" name="newPasswordRepeat" placeholder="Repeat new password"/></span> <span>&nbsp </span><span class="profilePageSubmitButton"><input type="submit" name="changePassword" value="Change password"/></span></form>';
}

function landingPageChangeToLoginForm()
{
	document.getElementById("landingPageForm").innerHTML = landingPageLoginFormLoad();
}
function landingPageChangeToRegisterForm()
{
	document.getElementById("landingPageForm").innerHTML = landingPageRegisterFormLoad();
}
function landingPageChangeToForgotLoginData()
{
	document.getElementById("landingPageForm").innerHTML = landingPageForgotLoginFormLoad();
}
function profilePageMainForm()
{
	document.getElementById("profilePageMainForm").innerHTML = profilePageMainFormLoad();
}

function topNavPage()
{
	document.getElementById("frontPageTopNav").innerHTML = topNavPageLoad();
}



function openFolder(folder)
{
	var active = document.querySelector(".selectedFolder"); 
	if(active!=null)active.classList.remove("selectedFolder");
	
	//for(var i=0; i<document.getElementsByClass("folderElement").length; i++)
	//	document.getElementsByClass("folderElement")[i].className+="selectedFolder";
	document.getElementsByName(folder)[0].className+=" selectedFolder";
	
	document.getElementById("fileListTable").innerHTML = getFolderContent(folder);
}

// placeholder
function getFolderContent(folder)
{
	if(folder=="folder1")
		return '<tr><th id="fileListTableFileName">File Name</th><th>File Type</th><th>File Size</th><th>Date Modified</th><th>Sharing</th></tr><tr name="file1" onclick="openFile(\'file1\')" class="fileElement selectedFile"><td>burek.jpg</td><td>JPG</td><td>9999 MB</td><td>1/1/1990</td><td>No</td></tr><tr name="file2" onclick="openFile(\'file2\')" class="fileElement"><td>kebab.jpg</td><td>TXT</td><td>222 KB</td><td>30/3/1933</td><td>Yes</td></tr>';
	if(folder=="folder2")
		return '<tr><th id="fileListTableFileName">File Name</th><th>File Type</th><th>File Size</th><th>Date Modified</th><th>Sharing</th></tr><tr name="file1" onclick="openFile(\'file1\')" class="fileElement selectedFile"><td>hamburger.WTF</td><td>WTF</td><td>1 B</td><td>23/11/2000</td><td>Yes</td></tr>';
	return '<tr><th id="fileListTableFileName">File Name</th><th>File Type</th><th>File Size</th><th>Date Modified</th><th>Sharing</th>';
}

function openFile(file)
{
	var active = document.querySelector(".selectedFile"); 
	if(active!=null)active.classList.remove("selectedFile");
	document.getElementsByName(file)[0].className+=" selectedFile";
	
	
	document.getElementById("sharingLink").value = "www.superduperwebpage.qwe/share?file=wq234"+file+"2d";
	document.getElementById("wholeName").value = ""+file+"";
}

function downloadFile()
{
	// get currently active file
	// send it to the user
	alert("downloading file placeholder");
}
