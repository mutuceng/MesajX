# 📬 MESAJX – Real-Time Chat Application

**MESAJX**, mikroservis mimarisi ile geliştirilmiş gerçek zamanlı bir mesajlaşma uygulamasıdır.  
Uygulama, kullanıcıların gruplar oluşturup arkadaşlarıyla anlık olarak iletişim kurmasını sağlar.  
Performans, ölçeklenebilirlik ve modülerlik göz önünde bulundurularak tasarlanmıştır.

Nasıl çalıştığını video üzerinden görmek için [LinkedIn Gönderim](https://www.linkedin.com/posts/umut-tanr%C4%B1verdi-2035b8259_microservices-rabbitmq-signalr-activity-7334563899133837312-tYeI?utm_source=share&utm_medium=member_desktop&rcm=ACoAAD-gfiMBPd1Gt_j_-FMlglQRWASSpXo4zQ4)'i ziyaret edebilirsiniz.

![Mesajlaşma](https://github.com/mutuceng/MesajX/blob/main/Docs/images/mesajxreeltime.png)
---

## 🧱 Mimarideki Servisler

Proje toplamda 3 ana mikroservisten oluşmaktadır:

- **User Service**  
  Kullanıcı yönetimi, kimlik doğrulama ve yetkilendirme işlemleri.  
  Duende IdentityServer kullanılmıştır.

- **Chat Service**  
  Mesaj gönderme, alma ve grup yönetimi işlemlerini yürütür.
  Katmanlı mimariye sahiptir.
  Redis + PostgreSQL veritabanı yapısıyla çalışır. M  

- **Sync Service**  
  Redis’te geçici olarak saklanan mesajları alır ve PostgreSQL'e aktarır.
  RabbitMQ olaylarını dinleyerek replikasyon işlemi yapar.

 ![Docker Containers](https://github.com/mutuceng/MesajX/blob/main/Docs/images/mesajdocker.png)

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|----------|----------|
| **C# (.NET)** | Tüm backend servislerinde kullanılan ana programlama dili |
| **React + TypeScript** | Frontend için modern ve type-safe kullanıcı arayüzü |
| **SignalR** | Gerçek zamanlı mesaj iletimi |
| **RabbitMQ** | Servisler arası mesajlaşma ve event-based yapı |
| **Redis** | Mesajların geçici olarak saklanması (1 gün TTL) ve grup üyelik kontrolü |
| **PostgreSQL** | Kalıcı veri saklama için ilişkisel veritabanı |
| **Duende IdentityServer** | Kimlik doğrulama ve token yönetimi |
| **YARP (Yet Another Reverse Proxy)** | Servis yönlendirme, gateway çözümü |

---

## 🔄 Mesaj Akışı

1. Kullanıcı bir mesaj gönderdiğinde, **Chat Service** mesajı Redis'e kaydeder.
2. Aynı anda `MessageCreatedEvent` RabbitMQ üzerinden yayınlanır.
3. **SignalR**, mesajı gruptaki diğer kullanıcılara gerçek zamanlı olarak iletir.
4. **Sync Service**, bu olayı dinler ve mesajı PostgreSQL'e yedekler.

---

## 📁 Proje Yapısı (Örnek)

```plaintext
MESAJX
├── .github
├── Backend
│   ├── .vs
│   ├── ApiGateway
│   ├── IdentityServer
│   ├── RabbitMQShared
│   ├── Services
│   │   ├── ChatService
│   │   │   ├── MesajX.ChatService
│   │   │   ├── MesajX.ChatService.BusinessLayer
│   │   │   ├── MesajX.ChatService.DataAccessLayer
│   │   │   ├── MesajX.ChatService.DtoLayer
│   │   │   ├── MesajX.ChatService.EntityLayer
│   │   ├── SyncService
├── docker-compose.yml
├── MesajX.sln
├── MesajX.sln.Launch
├── Web
│   ├── mesajXUI
