CREATE USER 'webapi_development' @'%' IDENTIFIED BY 'webapi_development_passwd';
GRANT ALL PRIVILEGES ON blog_development.* TO 'webapi_development' @'%';
