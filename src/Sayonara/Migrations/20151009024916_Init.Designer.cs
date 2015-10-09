using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using Sayonara.Models;

namespace Sayonara.Migrations
{
    [ContextType(typeof(SayonaraContext))]
    partial class Init
    {
        public override string Id
        {
            get { return "20151009024916_Init"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta5-13549"; }
        }
        
        public override void BuildTargetModel(ModelBuilder builder)
        {
            builder
                .Annotation("SqlServer:DefaultSequenceName", "DefaultSequence")
                .Annotation("SqlServer:Sequence:.DefaultSequence", "'DefaultSequence', '', '1', '10', '', '', 'Int64', 'False'")
                .Annotation("SqlServer:ValueGeneration", "Sequence");
            
            builder.Entity("Sayonara.Models.Extract", b =>
                {
                    b.Property<int>("ID")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity)
                        .Annotation("SqlServer:ValueGeneration", "Identity");
                    
                    b.Property<DateTime>("CompletionDate");
                    
                    b.Property<string>("CreatedBy");
                    
                    b.Property<int>("CurrentCount");
                    
                    b.Property<int>("DocumentationViewID");
                    
                    b.Property<DateTime>("ExtractionDate");
                    
                    b.Property<int>("FacilityID");
                    
                    b.Property<string>("FilePath");
                    
                    b.Property<int>("Format");
                    
                    b.Property<DateTime>("ReceivedDate");
                    
                    b.Property<DateTime>("ShippedDate");
                    
                    b.Property<string>("Status");
                    
                    b.Property<int>("TotalCount");
                    
                    b.Key("ID");
                });
        }
    }
}
