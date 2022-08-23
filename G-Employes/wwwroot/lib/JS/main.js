function ExitMessage() {
    document.getElementById('messageNotify').removeAttribute;
}

function changeOption() {
    var valueCat = document.getElementById("catFonc").value;

    if (valueCat == 'overier') {
        document.getElementById('typeFonc').style.display = 'block';
        document.getElementById('lbl').style.display = 'block';
    }
    else {
        document.getElementById('typeFonc').style.display = 'none';
        document.getElementById('lbl').style.display = 'none';

    }
  
   
}

