using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorApp1.Data;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.CompilerServices;

namespace BlazorApp1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Thread myThread = new Thread(new ThreadStart(Count));
            myThread.Start(); // запускаем поток
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
        public static void Count() 
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Ќазначаем сокет локальной конечной точке и слушаем вход€щие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                while (true)
                {
                    Socket handler = sListener.Accept();
                    string data = null;

                    // ћы дождались клиента, пытающегос€ с нами соединитьс€

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // ѕоказываем данные на консоли
                    string memory = data.Substring(0,1);
                    data = data.Remove(0, 1);
                    //1 - Starting program clients
                    //2 - AD Counter Win+Lin
                    //3 - User Couner
                    //4 - GPO Couner
                    //0 - Statistic sender
                    switch (memory)
                    {
                        case "1":
                            string[] words = data.Split(new char[] { '\\' });

                            string path = AppDomain.CurrentDomain.BaseDirectory;
                            string relPath = @"..\..\..\Memory\ProtectedCode.txt";
                            string resPath = Path.Combine(path, relPath);
                            resPath = Path.GetFullPath(resPath);
                            int count = File.ReadAllLines(resPath).Length;
                            string[] coun = File.ReadAllLines(resPath);

                            int row_count = count;
                            int col_count = 4;
                            string[,] str_array = new string[row_count, col_count];

                            string one = string.Join("\\", coun);
                            string[] tords = one.Split(new char[] { '\\' });
                            for (int i = 0; i < count*4; i+=4)
                            {
                                if (words[2] == tords[i])
                                { 
                                    words[2] = "Error CheckSum not received|wrong";
                                    string self_destruction = "Start self_destruction";
                                    byte[] self_destruction_sender = Encoding.UTF8.GetBytes(self_destruction);
                                    handler.Send(self_destruction_sender);
                                    break;
                                }
                                else { words[2] = "Check"; }
                            }
                            path = AppDomain.CurrentDomain.BaseDirectory;
                            relPath = @"..\..\..\Memory\IPbase.txt";
                            resPath = Path.Combine(path, relPath);
                            resPath = Path.GetFullPath(resPath);
                            string str = words[1] + "\\" + words[0] + "\\" + words[2] + "\r\n";
                            using (StreamWriter swriter = new StreamWriter(resPath, true))
                            {
                                swriter.Write(str);
                            }
                            break;
                        case "2":
                            Temp(@"..\..\..\Memory\Clicker_AD.txt", data);
                            break;
                        case "3":
                            Temp(@"..\..\..\Memory\UserCouner.txt", data);
                            break;
                        case "4":
                            Temp(@"..\..\..\Memory\GPOCouner.txt", data);
                            break;
                        case "0":
                            string[] pepe = new string[] { @"..\..\..\Memory\Clicker_AD.txt", @"..\..\..\Memory\UserCouner.txt", @"..\..\..\Memory\GPOCouner.txt" };
                            string reply = "";
                            for (int i = 0; i != 3; i++)
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory;
                                relPath = pepe[i];
                                resPath = Path.Combine(path, relPath);
                                resPath = Path.GetFullPath(resPath);
                                string[] counte = File.ReadAllLines(resPath);
                                reply += counte[0] + "\\";
                            }
                            int q = reply.Length - 1;
                            reply = reply.Remove(q);
                            byte[] msg = Encoding.UTF8.GetBytes(reply);
                            handler.Send(msg);
                            break;
                        default:
                            break;
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch{}
        }
        public static void Temp(string temp, string data)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string relPath = temp;
            string resPath = Path.Combine(path, relPath);
            resPath = Path.GetFullPath(resPath);
            string[] counte = File.ReadAllLines(resPath);
            int pepe = Convert.ToInt32(counte[0]);
            pepe += Convert.ToInt32(data);
            counte[0] = $"{pepe}";
            using FileStream fs = File.Create(resPath);
            byte[] info = new UTF8Encoding(true).GetBytes(counte[0]);
            fs.Write(info, 0, info.Length);
        }
    }
}
