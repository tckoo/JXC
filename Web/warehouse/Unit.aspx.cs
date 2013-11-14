using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Dictionary;
using ZLZJ.Entitys;
using System.Data;

public partial class warehouse_Unit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindProductUnit();
        }
    }

    /// <summary>
    /// 绑定产品类别
    /// </summary>
    private void BindProductUnit()
    {
        DictionaryBLL bll = new DictionaryBLL();

        DataTable dt = bll.GetDictionarysForUser("DW");

        aspCtg.RecordCount = dt.Rows.Count;
        PagedDataSource pds = new PagedDataSource();
        pds.AllowPaging = true;
        pds.PageSize = aspCtg.PageSize;
        pds.CurrentPageIndex = aspCtg.CurrentPageIndex - 1;
        pds.DataSource = dt.DefaultView;

        rptCtg.DataSource = pds;
        rptCtg.DataBind();
    }

    /// <summary>
    /// 添加计量单位
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DictionaryBLL bll = new DictionaryBLL();
        T_Dictionary obj = new T_Dictionary();
        obj.F_DictionaryID = Guid.NewGuid();
        obj.F_DicName = txtName.Value;
        obj.F_DicCode = txtCode.Value;
        obj.F_ParID = bll.GetDicIDByCode("DW");
        obj.F_IsPublic = 1;
        obj.F_ObjectID = Guid.Parse(Session["obj_id"] + "");
        obj.F_Status = byte.Parse(ddlStatus.SelectedValue);
        obj.F_AddDate = DateTime.Now;

        if (bll.AddDictionary("DW", Config.KEY_DW, obj))
        {
            BindProductUnit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('计量单位添加成功.');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('计量单位添加失败.');", true);
        }
    }

    /// <summary>
    /// 修改状态事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptCtg_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string cmd = e.CommandName, info;
        Guid dicID = Guid.Parse(e.CommandArgument + "");
        DictionaryBLL bll = new DictionaryBLL();
        byte status;
        if (cmd == "use" || cmd == "unuse")
        {
            fEdit.Style["display"] = "none";
            ViewState["dicID"] = "";
            if (cmd == "use")
            {
                status = 0;
                info = "启用";
            }
            else
            {
                status = 1;
                info = "停用";
            }

            if (bll.SetDictionaryStatus(dicID, status))
            {
                BindProductUnit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('计量单位" + info + "成功.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('计量单位" + info + "失败.');", true);
            }
        }
        // 编辑
        else if (cmd == "edit")
        {
            fEdit.Style["display"] = "";
            ViewState["dicID"] = dicID;
            T_Dictionary t = bll.GetDictionary(dicID);
            if (t == null)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('未知错误, 请稍后重试.');", true);
            }
            else
            {
                txtNName.Value = t.F_DicName;
                txtNCode.Value = t.F_DicCode;
                foreach (ListItem li in ddlNStatus.Items)
                {
                    li.Selected = li.Value == t.F_Status + "";
                }
            }
        }
    }

    /// <summary>
    /// 编辑计量单位
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNSave_Click(object sender, EventArgs e)
    {
        DictionaryBLL bll = new DictionaryBLL();
        T_Dictionary obj = new T_Dictionary();
        obj.F_DictionaryID = Guid.Parse(ViewState["dicID"] + "");
        obj.F_DicName = txtNName.Value;
        obj.F_DicCode = txtNCode.Value;
        obj.F_ParID = bll.GetDicIDByCode("DW");
        obj.F_Status = byte.Parse(ddlNStatus.SelectedValue);

        if (bll.EditDictionary(obj))
        {
            fEdit.Style["display"] = "none";
            ViewState["dicID"] = "";
            BindProductUnit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('计量单位保存成功.');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('计量单位保存失败.');", true);
        }
    }
    protected void aspCtg_PageChanged(object sender, EventArgs e)
    {
        BindProductUnit();
    }
}