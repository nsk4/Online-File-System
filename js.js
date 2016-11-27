var isLocationEnabled = true;

function loadWebpage()
{
	if(document.getElementsByTagName("footer").length>0) document.getElementsByTagName("footer")[0].innerHTML = loadFooter();
	if(document.getElementById("frontPageTopNav")!=null) document.getElementById("frontPageTopNav").innerHTML = topNavPageLoad();

	
	// we are on the main page
	if(document.getElementById("folderList")!=null && document.getElementById("fileListTable")!=null)	
	{
		getFolderStructString(fileFolderDataStructure.data);
		document.getElementById("folderList").innerHTML = folderStruct;
		for(var i in folderListText)
		{
			(function(i){
				//console.log("folder"+folderListID[i])
				document.getElementById("folder"+folderListID[i]).addEventListener("click", function(){ openFolder(folderListID[i]);}, false);
			})(i)
		}
			
		updateDrowdownMenus(); // update dropdowns
		/*fillFileTable(fileFolderDataStructure, folderListID[0]);*/
		openFolder(folderListID[0]);
		
		
		// adds login location
		if(isLocationEnabled) getLocation();
		
	}
	
	if(document.getElementById("accessListTable")!=null)
	{
		fillAccessList(pastLoginsDataStructure);
	}

	addEventListeners();

	
	
}

function addEventListeners()
{
	if(document.getElementById("loginFormLoginElement")!=null)
	{
		document.getElementById("loginFormLoginElement").addEventListener("click", function(){showPopup('loginForm');}, false);
		document.getElementById("loginFormRegisterElement").addEventListener("click", function(){showPopup('registerForm');}, false);
		document.getElementById("landingPageChangeLoginFormForgotData").addEventListener("click", function(){showPopup('forgotLoginForm');}, false);
	}
	
	if(document.getElementById("changeAccountSettingsEmailElement")!=null)
	{
		document.getElementById("changeAccountSettingsEmailElement").addEventListener("click", function(){showPopup('changeEmailPopup');}, false);
		document.getElementById("changeAccountSettingsPasswordElement").addEventListener("click", function(){showPopup('changePasswordPopup');}, false);
		document.getElementById("changeAccountSettingsLocationElement").addEventListener("click", function(){showPopup('changeLocationSettingsPopup');}, false);
		document.getElementById("changeAccountSettingsEmailCloseElement").addEventListener("click", function(){hidePopup('changeEmailPopup');}, false);
		document.getElementById("changeAccountSettingsPasswordCloseElement").addEventListener("click", function(){hidePopup('changePasswordPopup');}, false);
		document.getElementById("changeAccountSettingsLocationCloseElement").addEventListener("click", function(){hidePopup('changeLocationSettingsPopup');}, false);
	}
	
	
	
	if(document.getElementById("showFolderOptionsElement")!=null)
	{
		document.getElementById("showFolderOptionsElement").addEventListener("click", function(){showFolderOptions();}, false);
		document.getElementById("showFileOptionsElement").addEventListener("click", function(){showFileOptions();}, false);
		document.getElementById("frontPageFileInfoClosePopupElement").addEventListener("click", function(){hidePopup("frontPageFileInfoPopup");}, false);
		document.getElementById("downloadFilePopupCloseElement").addEventListener("click", function(){hidePopup("downloadFilePopup");}, false);
		document.getElementById("moveFilePopupCloseElement").addEventListener("click", function(){hidePopup("moveFilePopup");}, false);
		document.getElementById("deleteFilePopupCloseElement").addEventListener("click", function(){hidePopup("deleteFilePopup");}, false);
		document.getElementById("newFolderPopupCloseElement").addEventListener("click", function(){hidePopup("newFolderPopup");}, false);
		document.getElementById("renameFolderPopupCloseElement").addEventListener("click", function(){hidePopup("renameFolderPopup");}, false);
		document.getElementById("moveFolderPopupCloseElement").addEventListener("click", function(){hidePopup("moveFolderPopup");}, false);
		document.getElementById("deleteFolderPopupCloseElement").addEventListener("click", function(){hidePopup("deleteFolderPopup");}, false);
		document.getElementById("uploadFilePopupCloseElement").addEventListener("click", function(){hidePopup("uploadFileFolderPopup");}, false);
		document.getElementById("addSharedFilePopupCloseElement").addEventListener("click", function(){hidePopup("addSharedFileFolderPopup");}, false);
		document.getElementById("downloadFolderPopupCloseElement").addEventListener("click", function(){hidePopup("downloadFolderPopup");}, false);
	}
	

	
	
	
	
	
}

function loadFooter()
{
	return '<span class="halfWidth boxSpan">Created by Nejc Smrkolj Kozelj</span><span class="boxSpan" id="footerContactInfo">Contact information: <a href="mailto:nejcsk@hotmail.com">nejcsk@hotmail.com</a></span>';
}

function topNavPageLoad()
{
	return '<section id="frontPageTopNavLeft"><article><a href="frontPage.html" class="noneLink"><img src="Resources/logo.jpg" id="frontPageLogoImage" /></a></article></section><section id="frontPageTopNavRight"><article><a href="index.html" class="noneLink"><img src="Resources/exit.png" class="frontPageSmallImage" />Logout</a></article><article><a href="profilePage.html" class="noneLink"><img src="Resources/avatar.png" class="frontPageSmallImage" />Open profile</a></article></section>';
	
}

function updateDrowdownMenus()
{
	// folderListOptions
	var els = document.getElementsByClassName("folderListOptions");
	for(var i in els)
	{
		els[i].innerHTML = "";
		for(var j in folderListID)
		{
			els[i].innerHTML += "<option value='"+folderListID[j]+"'>"+folderListText[j]+"/</option>";
		}
		//console.log(els[i].innerHTML)
	}
}



function checkHash()
{
	fileFolderDataStructure.hash;
}




function getLocation() {
	//var x = document.getElementById("demo");
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
		alert("Geolocation is not supported by this browser.")
        //x.innerHTML = "Geolocation is not supported by this browser.";
    }
}
function showPosition(position) {
	/*
	var x = document.getElementById("demo");
    x.innerHTML = "Latitude: " + position.coords.latitude + 
    "<br>Longitude: " + position.coords.longitude; 
	//console.log(createQueryLink(position))
	*/
	//console.log(position);
	httpGetAsync(createQueryLink(position), addCountryToList);
	
}
function createQueryLink(position)
{
	return "https://maps.googleapis.com/maps/api/geocode/json?latlng="+position.coords.latitude+","+position.coords.longitude+"&sensor=false";
}


function httpGetAsync(theUrl, callback)
{
	var xmlHttp = new XMLHttpRequest();
	xmlHttp.onreadystatechange = function() { 
		if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
			callback(xmlHttp.responseText);
	}
	xmlHttp.open("GET", theUrl, true); // true for asynchronous 
	xmlHttp.send(null);
}

function addCountryToList(response)
{
	
	var obj = JSON.parse(response);
	if(obj.status!="OK")
	{
		// err in querying data
		alert("Error while getting GPS data.");
		return;
	}
	//console.log(obj);
	
	
	for (var i=0; i < obj.results[0].address_components.length; i++) 
	{
		for (var j=0; j < obj.results[0].address_components[i].types.length; j++) 
		{
			if (obj.results[0].address_components[i].types[j] == "country") 
			{
				country = obj.results[0].address_components[i];
				//console.log(country.long_name)
				//console.log(country.short_name)
				//console.log(obj);
				pastLoginsDataStructure.login.push({
					"timestamp":(new Date()).getTime(),
					"countryShort":country.short_name,
					"countryLong":country.long_name,
					"latitude":obj.results[0].geometry.location.lat,
					"longitude":obj.results[0].geometry.location.lng
				});
				//alert(country.long_name)
			}
		}
	}
	
	//document.getElementById("demo").innerHTML+=country.short_name + " " +country.long_name+"<br/>";
}


var pastLoginsDataStructure = {
	"login":[
	{
		"timestamp":1150212719856,
		"countryShort":"SI",
		"countryLong":"Slovenia",
		"latitude":46.056946499999995,
		"longitude":14.505751499999999
	},
	{
		
		"timestamp":1480212719856,
		"countryShort":"SI",
		"countryLong":"Slovenia",
		"latitude":46.056946499999995,
		"longitude":14.505751499999999
	}
	]
};

function fillAccessList(data)
{
	if(document.getElementById("accessListTable") == null) return;
	document.getElementById("accessListTable").innerHTML = "<tr><th>Date</th><th>Country</th></tr>";
	for(var i in data.login)
	{
		var date = new Date(data.login[i].timestamp);
		document.getElementById("accessListTable").innerHTML += "<tr><td>"+
			date.getDate() + "/"+
			(date.getMonth()+1) + "/"+
			date.getFullYear()
			+"</td><td>"+
			data.login[i].countryLong
			+"</td></tr>";
			
	}
}






var folderStruct = "";
var folderListID = [];
var folderListText = [];
function getFolderStructString(struct)
{
	//return;
	//console.log("started " + struct);
	var s1="";
	var s2="";
	for(var i in struct)
	{
		//console.log(struct[i].folderID + " " + struct[i].folderName)
		//console.log(struct[i]);
		
		folderStruct += "\
		<article class='folderElement'>\
		<span name='"+struct[i].folderID+"' id='folder"+struct[i].folderID+"' >\
		"+struct[i].folderName+"\
		</span>\
		";
		
		
		folderListID.push(struct[i].folderID);
		folderListText.push(struct[i].folderName);
		
		//folderStruct.innerHTML += s1;
		//console.log(s1)
		var el = struct[i].folders;
		//console.log(el);
		//console.log(struct.folders[i].folder)
		//console.log(htmlEl);
		//
		//if(el!=null)
		//{
		
		//}
		//*/
		getFolderStructString(el);
		
		
	}
	
	//console.log("fin "+struct)
	
	folderStruct += "</article>";
	//console.log(s2);
	//folderStruct.innerHTML += s2;
	
	//console.log(s1+" |||| "+s2)
}

//var fileStruct = "";
var fileListString = "";
var fileListIDs = [];
function fillFileTable(struct, folderID)
{
	//document.getElementById("fileListTable").innerHTML="qwe";
	//console.log(document.getElementById("fileListTable"));
	fileListIDs = [];
	fileListString = "\
			<th>Select</th>\
			<th id='fileListTableFileName'>File Name</th>\
			<th class='canHide'>File Type</th>\
			<th>File Size</th>\
			<th class='canHide'>Date Modified</th>\
			<th class='canHide'>Sharing</th>\
			<th>Options</th>\
			</tr>";
			
	//console.log("qwe "+folderID);		
	for(var i in struct)
	{
		// console.log(struct[i].folderID+" "+folderID)
		
		if(struct[i].folderID==folderID)
		{	
			for(var j in struct[i].files)
			{
				//struct[i].files[j].fileID
				//struct[i].files[j].fileName
				fileListString += "\
				<tr name='"+struct[i].files[j].fileID+"' class='fileElement'> \
				<td><input type='checkbox' name='fileCheckbox' id='file"+struct[i].files[j].fileID+"'  /></td> \
				<td name='fileName' class='canShorten'>"+struct[i].files[j].fileName+"</td>\
				<td name='fileType' class='canHide'>"+struct[i].files[j].fileType+"</td> \
				<td name='fileSize'>"+struct[i].files[j].fileSize+"</td> \
				<td name='fileModify' class='canHide'>"+struct[i].files[j].fileDateModified+"</td> \
				<td name='fileShare' class='canHide'>"+struct[i].files[j].fileSharing+"</td> \
				<td name='fileOptions'><img class='frontPageSmallImage' src='Resources/info.svg' id='filePopup"+struct[i].files[j].fileID+"' ></img></td>\
				</tr>";
				fileListIDs.push(struct[i].files[j].fileID);
			}
			//console.log(arr)
			//document.getElementById("fileListTable").innerHTML = arr;
			
			return;
		}
		else
		{
			var el = struct[i].folders;
			fillFileTable(el,folderID);
		}
	}
}


var fileInfoList = [];
function fillFileInfo(struct, fileID)
{
	for(var i in struct)
	{
		//console.log(i)
		for(var j in struct[i].files)
		{
			//console.log("qwe "+j+ " "+fileID)
			if(struct[i].files[j].fileID==fileID)
			{
				//console.log("ewq "+j+" "+struct[i].files[j].fileName)
				fileInfoList = [];
				fileInfoList.push(struct[i].files[j].fileID);
				fileInfoList.push(struct[i].files[j].fileName);
				fileInfoList.push(struct[i].files[j].fileType);
				fileInfoList.push(struct[i].files[j].fileSize);
				fileInfoList.push(struct[i].files[j].fileDateModified);
				fileInfoList.push(struct[i].files[j].fileSharing);
				fileInfoList.push(struct[i].files[j].fileSharingCode);
				//console.log("qwe");
				return;
			}
			
			
		}
		var el = struct[i].folders;
		fillFileInfo(el,fileID);
	}
	
}



var fileFolderDataStructure = 
{
	"hash":"34902349942342",
	"data":
	{
		"312313":{
			"folderID":"312313",
			"folderName":"root",
			"files":
			{
				"6168531" : {
					"fileID":"6168531",
					"fileName":"QQQQQQQQQQQQQQQQQQQQQburek.jpg",
					"fileType":"jpg",
					"fileSize":"999 MB",
					"fileDateModified":"1/1/1990",
					"fileSharing":"No",
					"fileSharingCode":"d320d234324f2"
				},
				"9513844" : {
					"fileID":"9513844",
					"fileName":"kebab.txt",
					"fileType":"txt",
					"fileSize":"11 KB",
					"fileDateModified":"2/12/2010",
					"fileSharing":"Yes",
					"fileSharingCode":"dwej32f3f"
				}
			},
			"folders":
			{
				
				"3242424": {
					"folderID":"3242424",
					"folderName":"1folder1",
					"files":
					{
						"54423324" : {
							"fileID":"54423324",
							"fileName":"qwe.jpg",
							"fileType":"jpg",
							"fileSize":"333 MB",
							"fileDateModified":"1/1/2020",
							"fileSharing":"No",
							"fileSharingCode":"fo2r3fc2jw0i"
						},
						"951423844" : {
							"fileID":"951423844",
							"fileName":"21qs.txt",
							"fileType":"txt",
							"fileSize":"11 GB",
							"fileDateModified":"31/1/1950",
							"fileSharing":"No",
							"fileSharingCode":"fk2930r32"
						},
						"432424234" : {
							"fileID":"432424234",
							"fileName":"bbbbb.qwe",
							"fileType":"qwe",
							"fileSize":"20 MB",
							"fileDateModified":"10/10/2011",
							"fileSharing":"Yes",
							"fileSharingCode":"fok23fwdq"
						}
					},
					"folders":
					{
						"324234242342": {
							"folderID":"324234242342",
							"folderName":"myFolder",
							"files": {
							},
							"folders":{	
							}
						}
					}
				},
				
				"34242342": {
					"folderID":"34242342",
					"folderName":"myFolder1",
					"files": {
					},
					"folders":{
					}
				}
			}
		}
		
	}
}
	
	

var activeFolder = "root";
function openFolder(folder)
{
	hideAllPopup();
	// gets folder files
	fillFileTable(fileFolderDataStructure.data, folder);
	document.getElementById("fileListTable").innerHTML=fileListString;
	
	for(var i in fileListIDs)
	{
		(function(i){
			//console.log("file"+fileListIDs[i])
			document.getElementById("file"+fileListIDs[i]).addEventListener("click", function(){ selectRemoveFile(fileListIDs[i]);}, false);
			
			document.getElementById("filePopup"+fileListIDs[i]).addEventListener("click", function(){ openFile(fileListIDs[i]);}, false);
		
		
		})(i)
	}
	
	
	// color
	activeFolder = folder;
	var active = document.querySelector(".selectedFolder"); 
	if(active!=null)active.classList.remove("selectedFolder");
	document.getElementsByName(folder)[0].className+=" selectedFolder";
	
	//for(var i=0; i<document.getElementsByClass("folderElement").length; i++)
	//	document.getElementsByClass("folderElement")[i].className+="selectedFolder";
	//document.getElementById("fileListTable").innerHTML = getFolderContent(folder);
}

function openFile(file)
{
	//var active = document.querySelector(".selectedFile"); 
	//if(active!=null) active.classList.remove("selectedFile");
	//document.getElementsByName(file)[0].className+=" selectedFile";
	
	
	//document.getElementById("sharingLink").value = "www.superduperwebpage.qwe/share?file=wq234"+file+"2d";
	/*document.getElementById("wholeName").value = ""+file+"";*/
	//showInfo(file);
	/*
	console.log("qwe");
	fileInfoList.push(struct[i].files[j].fileID);
	fileInfoList.push(struct[i].files[j].fileName);
	fileInfoList.push(struct[i].files[j].fileType);
	fileInfoList.push(struct[i].files[j].fileSize);
	fileInfoList.push(struct[i].files[j].fileDateModified);
	fileInfoList.push(struct[i].files[j].fileSharing);
	fileInfoList.push(struct[i].files[j].fileSharingCode);
	*/
	
	// sets the popup for file			
	fillFileInfo(fileFolderDataStructure.data, file);
	document.getElementById("fileInfoFileName").innerHTML = fileInfoList[1];
	document.getElementById("fileInfoFileType").innerHTML = fileInfoList[2];
	document.getElementById("fileInfoFileSize").innerHTML = fileInfoList[3];
	document.getElementById("fileInfoFileDateModified").innerHTML = fileInfoList[4];
	document.getElementById("fileInfoFileShared").innerHTML = fileInfoList[5];
	if(fileInfoList[5]=="Yes")
		document.getElementById("shareFileYes").checked = true;
	else
		document.getElementById("shareFileNo").checked = true;
	document.getElementById("sharingLink").value = "www.superduperwebpage.qwe/share?file="+fileInfoList[6];
	
	
	showPopup("frontPageFileInfoPopup");
}




var selectedFiles = [];
function selectRemoveFile(file)
{
	var selected = document.getElementsByName(file)[0];
	if(selected.className.search("selectedFile")>=0) // contains
	{
		var ind = selectedFiles.indexOf(selected);
		if(ind>=0)
		{
			selectedFiles.splice(ind, 1);
			selected.classList.remove("selectedFile");
		}
	}
	else
	{
		selected.className+=" selectedFile";
		selectedFiles.push(selected);
	}
}




function showFolderOptions()
{
	var el = document.getElementById("folderOptions");
	var selected = el.options[el.selectedIndex].value;
	switch(selected)
	{
		//// FOLDER
		case "newFolder":
			showPopup("newFolderPopup");
			break;
		case "renameFolder":
			showPopup("renameFolderPopup");
			break;
		case "moveFolder":
			showPopup("moveFolderPopup");
			break;
		case "deleteFolder":
			showPopup("deleteFolderPopup");
			break;
		case "uploadFile":
			showPopup("uploadFileFolderPopup");
			break;
		case "addSharedFile":
			showPopup("addSharedFileFolderPopup");
			break;
		case "downloadFolder":
			showPopup("downloadFolderPopup");
			break;
	}
}
function showFileOptions()
{
	var el = document.getElementById("fileOptions");
	var selected = el.options[el.selectedIndex].value;
	switch(selected)
	{
		//// FILE
		case "downloadFile":
			showPopup("downloadFilePopup");
			break;
		case "moveFile":
			showPopup("moveFilePopup");
			break;
		case "deleteFile":
			showPopup("deleteFilePopup");
			break;
	}
}


function showLoginPopup(popupId)
{
	showPopup(popupId);
}

function hideAllPopup()
{
	hidePopup("forgotLoginForm");
	hidePopup("loginForm");
	hidePopup("registerForm");
	
	hidePopup("changePasswordPopup");
	hidePopup("changeEmailPopup");
	hidePopup("changeLocationSettingsPopup");
	
	hidePopup("frontPageFileInfoPopup");
	hidePopup("downloadFilePopup");
	hidePopup("moveFilePopup");
	hidePopup("deleteFilePopup");
	
	hidePopup("newFolderPopup");
	hidePopup("renameFolderPopup");
	hidePopup("moveFolderPopup");
	hidePopup("deleteFolderPopup");
	hidePopup("uploadFileFolderPopup");
	hidePopup("addSharedFileFolderPopup");
	hidePopup("downloadFolderPopup");
}

function showPopup(popupId)
{
	hideAllPopup();
	
	document.getElementById(popupId).style.display = "block";
}
function hidePopup(popupId)
{
	if(document.getElementById(popupId)!=null)
		document.getElementById(popupId).style.display = "";
}