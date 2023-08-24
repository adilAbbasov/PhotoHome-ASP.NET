// Checking url change

var prevUrl = undefined;

setInterval(() => {
    const currUrl = window.location.href;

    if (currUrl != prevUrl) {
        prevUrl = currUrl;

        if (window.location.href.endsWith("/Home/Index") || window.location.href.endsWith('/')) {
            if (localStorage.getItem('searchPattern') !== null)
                localStorage.removeItem('searchPattern');

            if (localStorage.getItem('searchType') !== null)
                localStorage.removeItem('searchType');
        }

        var searchPattern = localStorage.getItem("searchPattern")

        if (searchPattern !== null) {
            $('#searchTxtBx').val(searchPattern);
        }
    }
}, 60);


// searchSuggestions design additions

$('#searchSuggestions').css({
    "width": $('#searchDiv').width()
})

$(window).resize(() => {
    if ($(window).width() <= 768 && $('#searchSuggestions').css('display') == 'block') {
        $("#searchSuggestions").fadeOut(150)
    }
})

$(window).resize(() => {
    $('#searchSuggestions').css({
        "width": $('#searchDiv').width()
    })
})

$('#searchTxtBx').focus(() => {
    $("#searchSuggestions").fadeIn(150)
})

$('#searchTxtBx').on("focusout", function (event) {
    var clickedElement = event.relatedTarget;

    if (!$(this).is(clickedElement) && !$("#searchTxtBx").is(clickedElement) && $(this).has(clickedElement).length === 0) {
        $("#searchSuggestions").fadeOut(150)
    }
});


// Animation for like image

var animateButton = function (e) {
    e.preventDefault;
    e.target.classList.remove('animate');
    e.target.classList.add('animate');

    setTimeout(function () {
        e.target.classList.remove('animate');
    }, 700);
};

var bubblyButtons = document.getElementsByClassName("bubbly-button");

for (var i = 0; i < bubblyButtons.length; i++) {
    bubblyButtons[i].addEventListener('click', animateButton, false);
}


// Downloading selected image

var downloadImage = (ele) => {
    downloadImage(ele.id, "Image");
    var option = ele.id;

    $.ajax({
        type: "POST",
        url: "/Home/DownloadImage",
        data: { Link: String(option) },
    });

    function downloadImage(url, name) {
        fetch(url)
            .then(resp => resp.blob())
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = url;
                a.download = name;
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
            })
    }
}


// Like selected image

var likeImage = (obj) => {
    var option = obj.id;

    $.ajax({
        type: "POST",
        url: "/Home/LikeImage",
        data: { link: String(option) },
    });

    obj.classList.toggle("redHeart")
}


// Unlike selected image

var unlikeImage = (obj) => {
    var option = obj.id;

    $.ajax({
        type: "POST",
        url: "/Home/LikeImage",
        data: { link: String(option) },
    });

    var parent = obj.parentNode.parentNode.parentNode.parentNode;
    parent.remove();
}


// Deleting selected image

var deleteImage = (obj) => {
    var option = obj.id;

    $.ajax({
        type: "POST",
        url: "/Home/DeleteImage",
        data: { link: String(option) },
    });

    var parent = obj.parentNode.parentNode.parentNode;
    parent.remove();
}


// Sending  tags to action

function sendData() {
    var items = [];
    $("#tagUl li").map(function () {
        items.push(this.innerText);
    });
    $.ajax({
        type: "POST",
        data: {
            list: items
        },
        url: "/Home/AddTag",
    })
}


// Adding search suggestions for particular searchPattern

$(function () {
    $('#searchTxtBx').keyup(function () {
        var searchTerm = $(this).val().toLowerCase();

        $.ajax({
            url: '/Home/GetTag',
            type: 'POST',
            data: { searchTerm: searchTerm },
            success: function (data) {
                $('#suggestionsUL').empty();
                $.each(data, function (i, item) {
                    var url = '/Home/Search?searchPattern=' + item + '&searchType=tag';
                    var anchor = $('<a>').attr('href', url).attr('onclick', 'sendTagData(this)').text(item);
                    var li = $('<li>').append(anchor);

                    $('#suggestionsUL').append(li);
                });
            }
        });
    });
});


//Sending tag data to the localstorage

function sendTagData(element) {
    var textContent = element.textContent;

    localStorage.setItem("searchPattern", textContent);
    localStorage.setItem("searchType", "tag");
}


//Sending category data to the localstorage

function sendCategoryData(element) {
    var textContent = element.textContent;

    localStorage.setItem("searchPattern", textContent);
    localStorage.setItem("searchType", "category");
}


// Sending search data to the action

function sendSearchData(event) {
    event.preventDefault();

    var textContent = $('#searchTxtBx').val();
    var currentUrl = window.location.href;
    var https = currentUrl.split('//')[0];
    var hostPort = currentUrl.split('//')[1].split('/')[0];
    var index = currentUrl.indexOf('searchType');

    if (index != -1) {
        var searchType = currentUrl.substring(index + 11)

        if (searchType == undefined) {
            searchType = 'tag';
        }
    }

    if (textContent.length > 0) {
        var newUrl = https + "//" + hostPort + "/Home/Search?searchPattern=" + textContent + "&searchType=" + searchType;
        window.location.href = newUrl;

        localStorage.setItem("searchPattern", textContent);
        localStorage.setItem("searchType", "tag");
    }
    else {
        var newUrl = https + "//" + hostPort;
        window.location.href = newUrl;
    }
}