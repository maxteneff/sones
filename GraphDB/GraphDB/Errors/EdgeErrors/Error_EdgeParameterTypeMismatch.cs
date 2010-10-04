/*
* sones GraphDB - Open Source Edition - http://www.sones.com
* Copyright (C) 2007-2010 sones GmbH
*
* This file is part of sones GraphDB Open Source Edition (OSE).
*
* sones GraphDB OSE is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, version 3 of the License.
* 
* sones GraphDB OSE is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with sones GraphDB OSE. If not, see <http://www.gnu.org/licenses/>.
* 
*/

using System;

using sones.Lib;
using sones.GraphDB.TypeManagement.BasicTypes;

namespace sones.GraphDB.Errors
{
    public class Error_EdgeParameterTypeMismatch : GraphDBEdgeError
    {
        public ADBBaseObject CurrentType { get; private set; }
        public ADBBaseObject[] ExpectedTypes { get; private set; }

        public Error_EdgeParameterTypeMismatch(ADBBaseObject currentType, params ADBBaseObject[] expectedTypes)
        {
            CurrentType = currentType;
            ExpectedTypes = expectedTypes;
        }

        public override string ToString()
        {
            return String.Format("The type [{0}] is not valid. Please use one of [{1}].", CurrentType.ObjectName, ExpectedTypes.ToAggregatedString(i => i.ObjectName));
        }
    }
}