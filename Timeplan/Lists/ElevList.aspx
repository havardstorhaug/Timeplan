<%@ Page Title="Elever" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ElevList.aspx.cs" Inherits="Timeplan.Lists.ElevList" %>

<%--<%@ OutputCache Duration="1000" VaryByParam="none"%>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />

    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Opprett ny elev" OnClick="CreateNewButton_Click" ID="CreateNewButtonTop" />

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:HiddenField ID="ScrollPosition" runat="server" Value="0" />

            <asp:ListView
                ID="ElevListView"
                runat="server"
                ItemPlaceholderID="ElevItem"
                OnSorting="ListViewEvents_Sorting">

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr class="listview-heading">
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Id" ID="IdLinkButton" runat="server" Visible="False">Id</asp:LinkButton>
                                    <div class="show-hide">
                                        <%--TODO: better descrition/view of show hide func--%>
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowIdLinkButton" Visible="False"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 11%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Navn" ID="NavnLinkButton" runat="server">Navn</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowNavnLinkButton"  ToolTip="Vis/Skjul Navn"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 5%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Klasse" ID="KlasseLinkButton" runat="server">Klasse</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowKlasseLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 5%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Trinn" ID="TrinnLinkButton" runat="server">Klassetrinn</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowTrinnLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 5%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="SkoleTimerPrUke" ID="SkoleTimerPrUkeLinkButton" runat="server">Skole t/uke</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowSkoleTimerPrUkeLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 20%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="HovedPedagog" ID="HovedPedagogLinkButton" runat="server">Hovedpedagog</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowHovedPedagogLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 5%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="BemanningsNormSkole" ID="BemanningsNormSkoleLinkButton" runat="server">Norm skole</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowBemanningsNormSkoleLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 11%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Sfo" ID="SfoLinkButton" runat="server">Sfo avdeling</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowSfoLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 5%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="SfoProsent" ID="SfoProsentLinkButton" runat="server">Sfo %</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowSfoProsentLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th style="width: 10%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="BemanningsNormSfo" ID="BemanningsNormSfoLinkButton" runat="server">Norm sfo</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowBemanningsNormSfoLinkButton"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    </div>
                                </th>
                                <th></th>
                                <th></th>
                                <th></th>
                            
                            </tr>
                            <tr id="ElevItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen elever funnet
                    </p>
                </EmptyItemTemplate>

                <%--TODO: whats this?--%>
                <%--<GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>--%>

                <ItemTemplate>
                    <tr class="row-standard">
                        <td>
                            <div class="form-group">
                                <asp:Button CssClass="form-control" runat="server" ID="IdButton" Text='<%# Eval("Id") %>' Visible="False"></asp:Button>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" runat="server" ID="NavnTextBox" MaxLength="100" Text='<%# Eval("Navn") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RequiredFieldValidator runat="server" ID="NavnRequiredFieldValidator" Display="Dynamic" ControlToValidate="NavnTextBox" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="KlasseDropDown"></asp:DropDownList>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="TrinnDropDown"></asp:DropDownList>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control " runat="server" ID="SkoleTimerPrUkeTextBox" ReadOnly="true" Text='<%# Eval("SkoleTimerPrUke") %>'></asp:TextBox>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="HovedPedagogDropDown"></asp:DropDownList>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="BemanningsNormSkoleDropDown"></asp:DropDownList>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="SfoDropDown"></asp:DropDownList>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" runat="server" MaxLength="3" ID="SfoProsentTextBox" Text='<%# Eval("SfoProsent") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RangeValidator runat="server" ID="SfoProsentRangeValidator" ControlToValidate="SfoProsentTextBox" Type="Double" MinimumValue="0" MaximumValue="100" Display="Dynamic" ErrorMessage="Ugyldig %" />
                                    <asp:RequiredFieldValidator runat="server" ID="SfoProsentRequiredFieldValidator" ControlToValidate="SfoProsentTextBox" Display="Dynamic" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="BemanningsNormSfoDropDown"></asp:DropDownList>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <%--<td class="show-hide">
                            <a title="Lagre endringer" class="btn btn-sm btn-default btn-save" CommandArgument='<%# Eval("Id") %>' runat="server" OnClick="SaveButton_Click">
                                <span class="glyphicon glyphicon-floppy-saved"></span>
                            </a>
                            <a title="Slett elev" class="btn btn-sm btn-default btn-delete" OnClientClick="return ConfirmDelete()" CommandArgument='<%# Eval("Id") %>' runat="server" OnClick="DeleteButton_Click">
                                <span class="glyphicon glyphicon-remove red"></span>
                            </a>
                            <a title="Rediger elev i detaljvisning" runat="server" class=" btn btn-sm btn-default btn-edit">
                                <span class="glyphicon glyphicon-pencil green"></span>
                            </a>
                        </td>--%>
                        <td>
                            <div class="form-group btn-save">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="SaveButton_Click" ID="SaveLinkButton" ToolTip="Lagre endringer">
                                    <span class="glyphicon glyphicon-floppy-saved"></span>
                                </asp:LinkButton>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-delete">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" 
                                    OnClientClick='<%# String.Format("javascript:return ConfirmDelete(\"{0}\")", Eval("Navn") != null ? Eval("Navn").ToString() : string.Empty) %>' 
                                    CommandArgument='<%# Eval("Id") %>' OnClick="DeleteButton_Click" ID="DeleteLinkButton" ToolTip="Slett elev">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </asp:LinkButton>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-edit">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="EditButton_Click" ID="EditLinkButton" ToolTip="Rediger elev i detaljvisning">
                                    <span class="glyphicon glyphicon-pencil"></span>
                                </asp:LinkButton>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>

                    </tr>
                </ItemTemplate>

            </asp:ListView>

        </div>

    </section>

    <br />

    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID ="SaveButtonBottom"
          OnClientClick="if (!Page_ClientValidate()){ return false; } this.disabled = true;" UseSubmitBehavior="false"
        />
    <%--OnClientClick="if (!Page_ClientValidate()){ return false; } this.disabled = true; this.value = 'Lagrer...';"--%>
    <%--OnClientClick="if (!Page_ClientValidate()){ return false; } this.disabled = true;" UseSubmitBehavior="false"--%>
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Opprett ny elev" OnClick="CreateNewButton_Click" ID="CreateNewButtonBottom" />

    <script type="text/javascript">

        function ConfirmDelete(name) {
            if (confirm("Er du sikker på at du vil slette elev '" + name + "'?")) {
                return true;
            }
            return false;
        }

        $(function () {

            //Retrieve and use the existing scroll position from the hidden field
            var scrollPosition = $('#<%= ScrollPosition.ClientID %>').val();
            $(window).scrollTop(scrollPosition);

            $(window).scroll(function () {
                var currentScrollPosition = $(window).scrollTop();
                $('#<%= ScrollPosition.ClientID %>').val(currentScrollPosition);
            });
        });

        //window.scrollTo = function (x, y) {

        //    //if (x != 0 || y != 0)
        //        return window.scrollTo(x, y);
        //}

        //(function () { var originalValidationSummaryOnSubmit = window.ValidationSummaryOnSubmit; window.ValidationSummaryOnSubmit = function (validationGroup) { var originalScrollTo = window.scrollTo; window.scrollTo = function () { }; originalValidationSummaryOnSubmit(validationGroup); window.scrollTo = originalScrollTo; } }());

        //function ScrollToBottom() {
        //    window.scrollTo(0, document.body.scrollHeight);
        //}

        //$(document).ready(function () {
        //$("#CreateNewButtonTop").click(function () {
        //        $('html, body').animate({
        //            scrollTop: $(document).height() - $(window).height()
        //        },
        //               0,
        //               "linear"
        //            );
        //    });
        //});

    </script>

</asp:Content>
