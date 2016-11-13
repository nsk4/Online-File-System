function loadWebpage()
{
	if(document.getElementsByTagName("footer").length>0)
		document.getElementsByTagName("footer")[0].innerHTML = loadFooter();
}

function loadFooter()
{
	return '<span class="boxSpan">Created by Nejc Smrkolj Kozelj</span><span class="boxSpan">Contact information: <a href="mailto:nejcsk@hotmail.com">nejcsk@hotmail.com</a></span>';
	
	
}

function landingPageLoginFormLoad()
{
	return '<form name="landingPageLoginForm" action="frontPage.html">	<span>		Username:	</span>	<span>		<input type="text" name="username" placeholder="Enter username" required />	</span>	<span>		Password:	</span>	<span>		<input type="password" name="password" placeholder="Enter password" required/>	</span>	<span class="landingPageChangeLoginForm" id="landingPageChangeLoginFormForgotData" onclick="landingPageChangeToForgotLoginData()">		Forgot username/password?	</span>	<span>		<input type="submit" name="login" value="Login"/>	</span></form>';
	
}

function landingPageRegisterFormLoad()
{
	return '<form name="landingPageRegisterForm" action="frontPage.html">	<span>		Username:	</span>	<span>		<input type="text" name="username" placeholder="Enter username" required/>	</span>	<span>		Password:	</span>	<span>		<input type="password" name="password" placeholder="Enter password" required/>	</span>		<span>		Repeat password:	</span>	<span>		<input type="password" name="passwordRepeat" placeholder="Repeat password" required/>	</span>		<span>		Email:	</span>	<span>		<input type="email" name="email" placeholder="Enter email" required/>	</span>		<span>		Repeat email:	</span>	<span>		<input type="email" name="emailRepeat" placeholder="Repeat email" required/>	</span>	<span>		Display name:	</span>	<span>		<input type="text" name="displayName" placeholder="Display name" required/>	</span>	<span>	</span>	<span>		<input type="submit" name="register" value="Register"/>	</span></form>';
}

function landingPageForgotLoginFormLoad()
{
	return '<form name="landingPageForgotLoginForm" action="frontPage.html">	<span>		Enter email:	</span>	<span>		<input type="email" name="email" placeholder="Enter email" required />	</span>	<span>	</span>	<span>		<input type="submit" name="forgotLogin" value="Recover"/>	</span></form>';
	
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