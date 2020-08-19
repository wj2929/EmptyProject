$.validator.adEmptyProjectethod("guidnotempty", function (value, element, param) {
    return value;
});

jQuery.validator.adEmptyProjectethod("regularexpression", function (value, element, param) {
    ///<summary>自定义正则表达式验证</summary>
    ///<param name='value' type='string'>验证元素值</param>
    ///<param name='element' type='object'>验证元素</param>
    ///<param name='param' type='object'>传入的自定义正则表达式</param>
    param = unescape(param);
    var regex = new RegExp(param);
    var b = this.optional(element);
    b = b == "dependency-mismatch" ? false : b;
    return b || (regex.test(value));
}, "格式验证错误");

jQuery.validator.adEmptyProjectethod("isguid", function (value, element) {
    ///<summary>验证Guid</summary>
    ///<param name='value' type='string'>验证元素值</param>
    ///<param name='element' type='object'>验证元素</param>
    return this.optional(element) || value.IsGuid();
}, "Guid格式验证错误");

jQuery.validator.adEmptyProjectethod("shortfilenamebyjscssextneed", function (value, element) {
    ///<summary>验证符合扩展名为css、js的短文件名</summary>
    ///<param name='value' type='object'>验证元素值</param>
    ///<param name='element' type='object'>验证元素</param>
    var reWithFileName = /^[^\/\\<>\*\?\:"\|\.#]{1,16}/;
    var reWithExtNeed = /.*?\.(js|css)$/;
    return reWithFileName.test(value) && reWithExtNeed.test(value);
}, "格式错误，要求以js、css作为扩展名的短文件名");

jQuery.validator.adEmptyProjectethod("shortfilename", function (value, element) {
    ///<summary>验证短文件名</summary>
    ///<param name='value' type='object'>验证元素值</param>
    ///<param name='element' type='object'>验证元素</param>
    var reWithFileName = /^[^\/\\<>\*\?\:"\|\.#]{1,16}/;
    return reWithFileName.test(value);
}, "格式错误，要求为短文件名");
