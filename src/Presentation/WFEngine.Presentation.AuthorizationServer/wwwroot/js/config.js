(function () {
  var primary = localStorage.getItem("primary") || "#7366ff";
  var secondary = localStorage.getItem("secondary") || "#f73164";

  window.CubaAdminConfig = {
    primary: primary,
    secondary: secondary,
  };
})();
