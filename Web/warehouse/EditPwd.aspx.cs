using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.Common;
using ZLZJ.BLL.Users;

public partial class warehouse_EditPwd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string oldPwd = Utils.EncryptByDESbase64(txtOldPwd.Value),
            newPwd = Utils.EncryptByDESbase64(txtNewPwd.Value),
            str;
        UsersBLL bll = new UsersBLL();
        bll.EditPwd(Guid.Parse(Session["user_id"] + ""), oldPwd, newPwd, out str);
        if (str == "unexist")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('对不起, 此帐号不存在.');", true);
        }
        else if (str == "errorpwd")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('对不起, 您的旧密码错误.');", true);
        }
        else if (str == "failed")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('对不起, 密码修改失败, 请稍后重试.');", true);
        }
        else
        {
            Session["user_id"] = null;
            Session["account"] = null;
            Session["user_type"] = null;
            Session["obj_id"] = null;
            Session["person_name"] = null;
            Session["user_type_name"] = null;
            Session.Abandon();
            Session.Clear();
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('密码修改成功, 请重新登录.');window.parent.href='Login.aspx';", true);
        }
    }
}