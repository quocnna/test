<%@ page contentType="text/html; charset=UTF-8" pageEncoding="UTF-8" %>
<!DOCTYPE html>
<html>
<head>
    <title>JSP - Hello World</title>
</head>
<body>
<h1><%= "Hello World!" %>
</h1>
<br/>
<a href="hello-servlet">Hello Servlet</a>

<%--<script>--%>
<%--    user = {--%>
<%--        "name": "Geeks for Geeks",--%>
<%--        "age": "23"--%>
<%--    };--%>
<%--    --%>
<%--    let options = {--%>
<%--        method: 'POST',--%>
<%--        mode: 'no-cors',--%>
<%--        headers: {--%>
<%--            'Content-Type': 'application/json;charset=utf-8'--%>
<%--        },--%>
<%--        body: JSON.stringify(user)--%>
<%--    };--%>
<%--    --%>
<%--    let fetchRes = fetch(--%>
<%--        "http://localhost:8082/second",--%>
<%--        options);--%>
<%--    --%>
<%--    fetchRes.then(res =>--%>
<%--        res.json()).then(d => {--%>
<%--        console.log(d)--%>
<%--    });--%>
<%--</script>--%>
</body>
</html>