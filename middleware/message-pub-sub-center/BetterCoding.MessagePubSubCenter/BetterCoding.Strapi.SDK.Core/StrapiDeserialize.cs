using BetterCoding.Patterns.FactoryMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterCoding.Strapi.SDK.Core
{
    public interface IStrapiDeserialize : IProcessor<string, StrapiObject>
    {

    }

    //public class StrapiStringDeserialize : IStrapiDeserialize
    //{

    //}
}
