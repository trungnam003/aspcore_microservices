namespace Product.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseApplication(this IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");

            });

            //app.UseHttpsRedirection();
            //app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

        }
    }
}
