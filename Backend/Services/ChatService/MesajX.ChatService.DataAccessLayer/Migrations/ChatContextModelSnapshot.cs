﻿// <auto-generated />
using System;
using MesajX.ChatService.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MesajX.ChatService.DataAccessLayer.Migrations
{
    [DbContext(typeof(ChatContext))]
    partial class ChatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MesajX.ChatService.EntityLayer.Entities.ChatRoom", b =>
                {
                    b.Property<string>("ChatRoomId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsGroup")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.HasKey("ChatRoomId");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("MesajX.ChatService.EntityLayer.Entities.ChatRoomMember", b =>
                {
                    b.Property<string>("ChatRoomMemberId")
                        .HasColumnType("text");

                    b.Property<string>("ChatRoomId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ChatRoomMemberId");

                    b.HasIndex("ChatRoomId");

                    b.ToTable("ChatRoomMembers");
                });

            modelBuilder.Entity("MesajX.ChatService.EntityLayer.Entities.Message", b =>
                {
                    b.Property<string>("MessageId")
                        .HasColumnType("text");

                    b.Property<string>("ChatRoomId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("IsRead")
                        .HasColumnType("integer");

                    b.Property<string>("MediaUrl")
                        .HasColumnType("text");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("MessageId");

                    b.HasIndex("ChatRoomId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MesajX.ChatService.EntityLayer.Entities.ChatRoomMember", b =>
                {
                    b.HasOne("MesajX.ChatService.EntityLayer.Entities.ChatRoom", "ChatRoom")
                        .WithMany("Members")
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatRoom");
                });

            modelBuilder.Entity("MesajX.ChatService.EntityLayer.Entities.Message", b =>
                {
                    b.HasOne("MesajX.ChatService.EntityLayer.Entities.ChatRoom", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MesajX.ChatService.EntityLayer.Entities.ChatRoom", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
