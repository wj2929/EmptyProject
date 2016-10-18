var langstrings;
var langz_code = "";
$(function () {
    if (langz_code !== "") {
        $.get($.format("/lang/{0}.json", langz_code == "zh" ? "zh_cn" : "en"), function (data) {
            try{
                langstrings = JSON.parse(data);
            }catch(e){
                langstrings  = data;
            }
        });
    }
});

function getLang(str) {
    if (typeof (langstrings) == "undefined" || typeof (langstrings[str]) == 'undefined')
        return str;
    else
        return langstrings[str];
}