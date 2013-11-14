using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Dictionary;
using ZLZJ.Entitys;
using System.Data;

public partial class warehouse_PCtg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCtg();
            BindProductCtg();
        }
    }

    /// <summary>
    /// 绑定产品类别
    /// </summary>
    private void BindProductCtg()
    {
        DictionaryBLL bll = new DictionaryBLL();

        DataTable dt = bll.GetDictionarysForUser("CP");

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
    /// 绑定产品分类
    /// </summary>
    private void BindCtg()
    {
        DictionaryBLL bll = new DictionaryBLL();
        List<T_Dictionary> list = bll.GetDictionarys("CP", Guid.Parse(Session["obj_id"] + ""));
        ddlCtg.DataSource = list;
        ddlCtg.DataValueField = "F_DictionaryID";
        ddlCtg.DataTextField = "F_DicName";
        ddlCtg.DataBind();

        ddlNCtg.DataSource = list;
        ddlNCtg.DataValueField = "F_DictionaryID";
        ddlNCtg.DataTextField = "F_DicName";
        ddlNCtg.DataBind();
    }

    /// <summary>
    /// 添加产品类别
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
        if (rbtType.SelectedValue == "0")
        {
            obj.F_ParID = bll.GetDicIDByCode("CP");
        }
        else
        {
            obj.F_ParID = Guid.Parse(ddlCtg.SelectedValue);
        }
        obj.F_IsPublic = 1;
        obj.F_ObjectID = Guid.Parse(Session["obj_id"] + "");
        obj.F_Status = byte.Parse(ddlStatus.SelectedValue);
        obj.F_AddDate = DateTime.Now;

        if (bll.AddDictionary("CP", Config.KEY_CPLB, obj))
        {
            BindProductCtg();
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('类别添加成功.');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('类别添加失败.');", true);
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
                BindProductCtg();
                ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('类别" + info + "成功.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('类别" + info + "失败.');", true);
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
                foreach (ListItem item in ddlNCtg.Items)
                {
                    if (item.Value == (t.F_ParID + ""))
                    {
                        item.Selected = true;
                        rbtnNType.Items[0].Selected = false;
                        rbtnNType.Items[1].Selected = true;
                        ddlNCtg.Enabled = true;
                        break;
                    }
                    rbtnNType.Items[0].Selected = true;
                    rbtnNType.Items[1].Selected = false;
                    item.Selected = false;
                    ddlNCtg.Enabled = false;
                }
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
    /// 编辑产品类别
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
        if (rbtnNType.SelectedValue == "0")
        {
            obj.F_ParID = bll.GetDicIDByCode("CP");
        }
        else
        {
            obj.F_ParID = Guid.Parse(ddlNCtg.SelectedValue);
        }
        obj.F_Status = byte.Parse(ddlNStatus.SelectedValue);

        if (bll.EditDictionary(obj))
        {
            fEdit.Style["display"] = "none";
            ViewState["dicID"] = "";
            BindProductCtg();
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('类别保存成功.');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('类别保存失败.');", true);
        }
    }
    protected void aspCtg_PageChanged(object sender, EventArgs e)
    {
        BindProductCtg();
    }
}