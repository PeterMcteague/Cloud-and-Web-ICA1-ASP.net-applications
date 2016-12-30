//Variables
var http = new XMLHttpRequest(); 
var coreUrl = "http://localhost:65026/api/"; //Base URL to work with for API requests
var buUrl = coreUrl + "BusinessUnit"; //URL for a businessunit request
var staffListUrl = coreUrl + "Staff/BusinessUnit/"; //URL for a staff list request
var staffDetailUrl = coreUrl + "Staff/"; //URL for a staff detail request

//The function to run when the window loads.
function start() {
    //Hide the list of staff.
    hideStaffList();
    //Send a call to get a list of business units when the browser is ready
    http.onreadystatechange = getBuList;
    sendRequest(buUrl);
}

//For sending requests to the api.
function sendRequest(url)
{
    //Send a request for a JSON file from a URL.
    http.open("GET", url);
    http.setRequestHeader("Accept", "application/json")
    http.send();
}

//Gets a list of business units.
function getBuList() {
    //If the JSON response has been returned
    if (http.readyState === 4 && http.status === 200) {
        //Parse the JSON as a list of businessunits.
        var buisinessUnits = JSON.parse(http.responseText);
        //If its not empty, display them.
        if (buisinessUnits !== null) {
            displayItemsInBuList(buisinessUnits);
        } else {
            displayError();
        }
    }
    //Or if there's a problem with the API
    else if (http.readyState == 4 && http.status == 0) {
        displayError()
    }
}

function displayError()
{
    //Just create some text in the table giving an error message.
    var table = document.getElementById("buList");
    table.innerHTML = "";
    var row = table.insertRow(0);
    var cell1 = row.insertCell(0);
    var row2 = table.insertRow(1);
    var cell2 = row2.insertCell(0);
    cell1.innerHTML = "Sorry the API could not be reached at this time.";
    cell2.innerHTML = "Please try again later.";
}

//Displays each item in the list of business units.
function displayItemsInBuList(arr) {
    //Get the buList element
    var table = document.getElementById("buList");
    //Create a table for it
    table.innerHTML = "";
    for (var i = 0; i < arr.length; i++) {
        //Create a new row
        var row = table.insertRow(0);
        //Insert 2 cells
        var cell1 = row.insertCell(0);       
        var cell2 = row.insertCell(1);
        //Put the title of the BU in the first cell and a link to list the staff of it in the second
        cell1.innerHTML = arr[i].title;
        //Make it so that the businessunitcode is pushed to getstaff when the link in cell2 is clicked on
        var id = arr[i].businessUnitCode;        
        cell2.innerHTML = "&nbsp&nbsp&nbsp&nbsp<a href='#'     id='" + id + "' " + " >List Staff</a>";
        document.getElementById(id).onclick = getStaff;
    }
}

//Send a JSON request to get the staff for the clicked on element.
function getStaff(e) {
    var staffUrl = staffListUrl + e.target.id;
    http.onreadystatechange = requestStaffList;
    sendRequest(staffUrl);
}

//Handling the JSON request to get the staff list.
function requestStaffList() {
    //If the api is all fine.
    if (http.readyState === 4 && http.status === 200) {
        //Parse the response text as staffList
        var staffList = JSON.parse(http.responseText);
        if (staffList !== null) {
            //Call displayStaffList
            displayStaffList(staffList);
        } else {
            hideStaffList();
        }
    }
    //Wont handle an error here as it should've happened already if there was one.
}

//Displays a list of staff in the businessunit
function displayStaffList(arr) {
    document.getElementById("staffListHeader").style.visibility = "visible";
    document.getElementById("Staffdetail").style.visibility = "hidden";
    var table = document.getElementById("staffList");
    table.style.visibility = "visible";
    if (arr !== null)
    {
        table.innerHTML = "";
        for (var i = 0; i < arr.length; i++) {   
            var row = table.insertRow(0);   
            var cell1 = row.insertCell(0);
            var cell2 = row.insertCell(1);
            cell1.innerHTML = arr[i].fullName;
            var id = arr[i].staffCode;
            cell2.innerHTML = "&nbsp&nbsp&nbsp&nbsp<a href='#'     id='" + id + "' " + " >Staff Detail</a>";
            document.getElementById(id).onclick = getStaffDetails;
        }
    }
}

//Send a JSON request to get staff details for the clicked on element.
function getStaffDetails(e) {
    var url = staffDetailUrl + e.target.id;
    http.onreadystatechange = requestStaffDetail;
    sendRequest(url);
}

//Handling the JSON request for staff details.
function requestStaffDetail() {
    //If the api is all fine and is happy with what was sent.
    if (http.readyState === 4 && http.status === 200) {
        var staffDetail = JSON.parse(http.responseText);
        if (staffDetail !== null) {
            displayStaffDetail(staffDetail);
        } else {
            hideStaffDetail();
        }
    }
}

//Displays staff details for the staff member clicked on.
function displayStaffDetail(staff) {
    document.getElementById("Staffdetail").style.visibility = "visible";
    if (staff !== null) {
        document.getElementById("staffCode").innerHTML = "Staff code: " + staff.staffCode;
        document.getElementById("staffFirstName").innerHTML = "First Name: " + staff.firstName;
        document.getElementById("staffMiddleName").innerHTML = "Middle Name: " + staff.middleName;
        document.getElementById("staffLastName").innerHTML = "Last Name: " + staff.lastName;
        document.getElementById("staffEmail").innerHTML = "Email Address: " + staff.emailAddress;
        document.getElementById("staffProfile").innerHTML = "Profile: " + staff.profile;
        document.getElementById("staffStartDate").innerHTML = "Start Date: " + staff.startDate;
        document.getElementById("staffDOB").innerHTML = "Date of Birth: " + staff.dob;
    }
}

//Hides the list of staff. Sends you back to business units.
function hideAll() {
    document.getElementById("buList").style.visibility = "hidden";
    hideStaffList();
}

//Hides the list of staff.
function hideStaffList() {
    document.getElementById("staffListHeader").style.visibility = "hidden";
    document.getElementById("staffList").style.visibility = "hidden";
    document.getElementById("Staffdetail").style.visibility = "hidden";
            
}

//Hides the staff details.
function hideStaffDetail() {
    document.getElementById("Staffdetail").style.visibility = "hidden";
}

//When the browser loads the site, call the start method.
window.onload = start;
