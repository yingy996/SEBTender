function addRow(parentDIV, questionDIV, answerDIV) {
    
    var division = document.getElementById(parentDIV);
    var questiondivision = document.getElementById(questionDIV);
    var childrenanswerdivision = document.getElementById(answerDIV);

    var questiondivisionclone = questiondivision.cloneNode(true);

    var rowCount = document.querySelectorAll('*[id^="questionDIV0"]').length;
    var currentQuesNum = 0;
    
    //find current number of question
    for(var i=1; i<=rowCount; i++){
        currentQuesNum = i;
    }
            

    //questiondivision id = "questionDIV0" + questioncount
    questiondivisionclone.id = "questionDIV0" + currentQuesNum;
	
    
    //checkbox id = "quescheckbox0" + questioncount
    questiondivisionclone.querySelector("#quescheckbox").id = "quescheckbox0" + currentQuesNum;
    questiondivisionclone.querySelector("#quescheckbox0" + currentQuesNum).checked = false;
	questiondivisionclone.querySelector("#quescheckbox0" + currentQuesNum).setAttribute("name", "#quescheckbox0" + currentQuesNum);
    
    //questiontitle id= "questiontitle0" + questioncount
    questiondivisionclone.querySelector("#question_title").id = "question_title0" + currentQuesNum;
	questiondivisionclone.querySelector("#question_title0" + currentQuesNum).setAttribute("name", "question_title0" + currentQuesNum);
    
    //dropdown elements id = id + question number
    questiondivisionclone.querySelector("#answertype").id = "answertype0" + currentQuesNum;
	questiondivisionclone.querySelector("#answertype0" + currentQuesNum).setAttribute("name", "answertype0" + currentQuesNum);
    
    
    
    //id for answer division's elements ends with 4-5 digits with the first half denoting the question number and the latter half denoting the answer number
    questiondivisionclone.querySelector("#answerDIV").id = "answerDIV0" + currentQuesNum + "00";
	
    questiondivisionclone.querySelector("#anscheckbox").id = "anscheckbox0" + currentQuesNum + "00";
	questiondivisionclone.querySelector("#anscheckbox0" + currentQuesNum + "00").setAttribute("name", "anscheckbox0" + currentQuesNum + "00");
    questiondivisionclone.querySelector("#answer_title").id= "answer_title0" + currentQuesNum + "00";
	questiondivisionclone.querySelector("#answer_title0" + currentQuesNum + "00").setAttribute("name", "answer_title0" + currentQuesNum + "00");

    questiondivisionclone.style.display = "block";
    if(rowCount < 20){
        division.appendChild(questiondivisionclone);
    }else{
        alert("Maximum survey questions are 20");
    }

    
}

function addAnswerRow(){

    var chkArray = [];

    
    /*if(document.getElementById("quescheckbox00").checked == true){
        chkArray.push(0);
    }*/

    //Only possible to have questions up to 20
    for(var i=0; i<=20; i++){
        if(document.getElementById("quescheckbox0" + i)!= null) {
            if(document.getElementById("quescheckbox0" + i).checked == true){
                var answertype = document.getElementById("answertype0" + i);
                if(answertype.options[answertype.selectedIndex].value != "shortsentence" && answertype.options[answertype.selectedIndex].value != "longsentence"){
                    chkArray.push(i);   
                }
            }
        }
    }    
                                  
    for(var i=0; i < chkArray.length; i++) {
            var currentQuesNumber = chkArray[i];

            var questiondivision = document.getElementById("questionDIV0" + currentQuesNumber);
            var answerdivision = document.getElementById("answerDIV0" + currentQuesNumber + "00");
            var answerdivisionclone = answerdivision.cloneNode(true);

            //set each checked answer division id to answerDIV0 + current question number
            var answerRowCount = document.querySelectorAll('*[id^="answerDIV0' + currentQuesNumber + '"]').length;

            //find current number of answers for this particular question
            for(var x=1; x<=answerRowCount; x++){
                var currentAnswerNumber = x;
            }
            
            //set every new answer's id in the format of 'answerDIV0' + current question number + '0' + current answer count number 
            answerdivisionclone.id = "answerDIV0" + currentQuesNumber + "0" + currentAnswerNumber;
			
		
            //set every new answer's checkbox id in the format of 'anscheckbox0' + current question number + '0' + currentAnswerNumber
            answerdivisionclone.querySelector("#anscheckbox0" + currentQuesNumber + "00").id = "anscheckbox0" + currentQuesNumber + "0" + currentAnswerNumber;
            answerdivisionclone.querySelector("#anscheckbox0" + currentQuesNumber + "0" + currentAnswerNumber).checked = false;
			answerdivisionclone.querySelector("#anscheckbox0" + currentQuesNumber + "0" + currentAnswerNumber).setAttribute("name", "anscheckbox0" + currentQuesNumber + "0" + currentAnswerNumber);
        
            //set every new answer's title id in the format of 'answer_title0' + current question number + '0' + currentAnswerNumber
            answerdivisionclone.querySelector("#answer_title0" + currentQuesNumber + "00").id = "answer_title0" + currentQuesNumber + "0" + currentAnswerNumber;
            answerdivisionclone.querySelector("#answer_title0" + currentQuesNumber + "0" + currentAnswerNumber).setAttribute("name", "answer_title0" + currentQuesNumber + "0" + currentAnswerNumber);
            
            
            questiondivision.appendChild(answerdivisionclone);
    }
    
}

//called whenever answertype dropdown is edited (resets the answer division to 1 if shortsentence or longsentence is chosen)
function answertypechange(selectObject){
    if(selectObject.options[selectObject.selectedIndex].value == "shortsentence" || selectObject.options[selectObject.selectedIndex].value == "longsentence"){
        var answertypeid = selectObject.id;
        var currentQuesNumber = answertypeid.substring(answertypeid.indexOf('0')+1);
        var questiondivisionid = "questionDIV0" + currentQuesNumber;
        var answerRowCount = document.querySelectorAll('*[id^="answerDIV0' + currentQuesNumber + '"]').length;
        
        for(var x=1; x<answerRowCount; x++){
                var currentanswerdiv = document.getElementById("answerDIV0" + currentQuesNumber + "0" + x);
                currentanswerdiv.parentNode.removeChild(currentanswerdiv);
            }
    }
    
}
function deletequestionRow() {
    
	
    var chkArray = [];

    //Only possible to have questions up to 20
    for(var i=0; i<=20; i++){
		var currentcheckboxrow = document.getElementById("quescheckbox0" + i);
        if(currentcheckboxrow != null) {
            if(currentcheckboxrow.checked == true){
				if(i != 0){
                    chkArray.push(i);
				}else{
                	alert("cannot delete question 1");
            	}	
				
            }
        }
    }    
    
	//for delete confirmation string
	var labelquestionnumber = "";
	for(var i=0; i<chkArray.length; i++){
		var currentnum = parseInt(chkArray[i]);
		if(i ==0){
			labelquestionnumber = (currentnum + 1).toString();
		}else{
			labelquestionnumber = labelquestionnumber + "," + (currentnum + 1).toString();
		}
	}
	
	var chkArrayLength = chkArray.length;
	if(window.confirm("Are you sure you want to delete questions :" + labelquestionnumber) == true){
		for(var x=0; x<chkArrayLength; x++){

				var question = document.getElementById("questionDIV0" + chkArray[0]);
				question.parentNode.removeChild(question);
				chkArray.splice(chkArray.indexOf(chkArray[0]), 1);
		 }
	}
    
    
    //after deletions, rearrange questions and their answer divs in ascending order
    var questionrows = document.querySelectorAll('*[id^="questionDIV0"]');
    var questioncheckboxrows = document.querySelectorAll('*[id^="quescheckbox0"]');
    var questiontitlerows = document.querySelectorAll('*[id^="question_title0"]');
    var answertyperows = document.querySelectorAll('*[id^="answertype0"]');
    for(var i=0; i<questionrows.length; i++){
            var questionnum = i;
            
        
            questionrows[i].id = "questionDIV0" + questionnum;
			
            questioncheckboxrows[i].id = "quescheckbox0" + questionnum;
			questioncheckboxrows[i].setAttribute("name", "quescheckbox0" + questionnum);
			questioncheckboxrows[i].checked = false;
            questiontitlerows[i].id = "question_title0" + questionnum;    
			questiontitlerows[i].setAttribute("name", "question_title0" + questionnum);
            answertyperows[i].id = "answertype0" + questionnum;
			answertyperows[i].setAttribute("name", "answertype0" + questionnum);
        
            var answerrows = document.querySelectorAll('*[id^="answerDIV0' + questionnum + '0"]');
            var answercheckboxrows = document.querySelectorAll('*[id^="anscheckbox0' + questionnum + '0"]');
            var answertitlerows = document.querySelectorAll('*[id^="answer_title0' + questionnum + '0"]');
            //answerrows.length == 0 means that current question number was deleted hence the next question's answers need to replace the deleted ones
            if(answerrows.length > 0){
                for(var x=0; x<answerrows.length; x++){
                    answerrows[x].id = "answerDIV0" + questionnum + "0" + x;
					
                    answercheckboxrows[x].id = "anscheckbox0" + questionnum + "0" + x;
					answercheckboxrows[x].setAttribute("name", "anscheckbox0" + questionnum + "0" + x);
                    answertitlerows[x].id = "answer_title0" + questionnum + "0" + x;
					answertitlerows[x].setAttribute("name", "answer_title0" + questionnum + "0" + x);
                }
            }else{
                var nextquestionnumber = questionnum + 1;
                answerrows = document.querySelectorAll('*[id^="answerDIV0' + nextquestionnumber + '0"]');
				answercheckboxrows = document.querySelectorAll('*[id^="anscheckbox0' + nextquestionnumber + '0"]');
            	answertitlerows = document.querySelectorAll('*[id^="answer_title0' + nextquestionnumber + '0"]');
                for(var x=0; x<answerrows.length; x++){
                    answerrows[x].id = "answerDIV0" + questionnum + "0" + x;
					
                    answercheckboxrows[x].id = "anscheckbox0" + questionnum + "0" + x;
					answercheckboxrows[x].setAttribute("name", "anscheckbox0" + questionnum + "0" + x);
                    answertitlerows[x].id = "answer_title0" + questionnum + "0" + x;
					answertitlerows[x].setAttribute("name", "answer_title0" + questionnum + "0" + x);
                }
            }
    }
    /*var allquestionrows = document.querySelectorAll('*[id^="questionDIV0"]');
    for(var i=0; i<allquestionrows.length; i++){
        var allanswerrows = document.querySelectorAll('*[id^="answerDIV0' + i + '0"]');
        for(var x=0; x<allanswerrows.length; x++){
            alert(allanswerrows[x].id);
        }
    }*/
    
}

function deleteanswerRows(){
    var chkArray = {};
    if(window.confirm("Are you sure you want to delete selected answer(s)?")){
    
		//Only possible to have questions up to 20
		for(var i=0; i<=20; i++){
			var questionnumber = i.toString();
			chkArray[questionnumber] = new Array();
			if(document.getElementById("questionDIV0" + i)!= null) {
				var answerrowcount = document.querySelectorAll('*[id^="answerDIV0' + i + '0"]').length;
				for(var x=0; x< answerrowcount; x++){
					if(document.getElementById("anscheckbox0" + i + "0" + x).checked == true){
						if(x != 0){
				    		
                    		chkArray[questionnumber].push(x);
						}else{
							alert("Cannot delete first instance of answer field in each question");	
						}
					}
				}
			}
		}

		
		var numberofquestioninArray = Object.keys(chkArray).length;
		
		//delete answer fields
		for(var questionnumberkey in chkArray){
			var numberofanswerinthisQuestion = chkArray[questionnumberkey].length;
			if(numberofanswerinthisQuestion > 0){
            	for(var x=0; x<numberofanswerinthisQuestion; x++){
                	var answer = document.getElementById("answerDIV0" + questionnumberkey + "0" + chkArray[questionnumberkey][0]);
                	answer.parentNode.removeChild(answer);
                	chkArray[questionnumberkey].splice(chkArray[questionnumberkey].indexOf(chkArray[questionnumberkey][0]), 1);
                }
			}
        }
        //after deletions, rearrange questions and their answer divs in ascending order
		var questionrows = document.querySelectorAll('*[id^="questionDIV0"]');
		var questioncheckboxrows = document.querySelectorAll('*[id^="quescheckbox0"]');
		var questiontitlerows = document.querySelectorAll('*[id^="question_title0"]');
		var answertyperows = document.querySelectorAll('*[id^="answertype0"]');
		for(var i=0; i<questionrows.length; i++){
				var questionnum = i;
				

				questionrows[i].id = "questionDIV0" + questionnum;
				
				questioncheckboxrows[i].id = "quescheckbox0" + questionnum;
				questioncheckboxrows[i].setAttribute("name", "quescheckbox0" + questionnum);
				questioncheckboxrows[i].checked = false;
				questiontitlerows[i].id = "question_title0" + questionnum;    
				questiontitlerows[i].setAttribute("name", "question_title0" + questionnum);
				answertyperows[i].id = "answertype0" + questionnum;
				answertyperows[i].setAttribute("name", "answertype0" + questionnum);

				var answerrows = document.querySelectorAll('*[id^="answerDIV0' + questionnum + '0"]');
				var answercheckboxrows = document.querySelectorAll('*[id^="anscheckbox0' + questionnum + '0"]');
				var answertitlerows = document.querySelectorAll('*[id^="answer_title0' + questionnum + '0"]');
				
				if(answerrows.length > 0){
					for(var x=0; x<answerrows.length; x++){
						answerrows[x].id = "answerDIV0" + questionnum + "0" + x;
						
						answercheckboxrows[x].id = "anscheckbox0" + questionnum + "0" + x;
						answercheckboxrows[x].setAttribute("name", "anscheckbox0" + questionnum + "0" + x);
						answertitlerows[x].id = "answer_title0" + questionnum + "0" + x;
						answertitlerows[x].setAttribute("name", "answer_title0" + questionnum + "0" + x);
					}
				}else{
					alert("caonima");
				}
		}
    }    
}

/*function surveyNextButton(){
	//var startdate = document.getElementById("startdate").value;
	var enddate = document.getElementById("enddate").value;
	if(!enddate){
		alert("Enddate cannot be null");
	}
}*/