# Example Outputs

So as the serializers within here are interchangable as far as the pipelines go here is an extract from the tests which shows the same object being spat out through 3 different serializers:

---

## JSON
This is about 1kb in size when in a file
```json
{
   "TestValue":"WOW",
   "NestedValue":{
      "StringValue":"Hello",
      "IntValue":0,
      "NestedArray":[
         {
            "FloatValue":2.4300000667572
         }
      ]
   },
   "NestedArray":[
      {
         "StringValue":"There",
         "IntValue":20,
         "NestedArray":[
            {
               "FloatValue":3.5
            }
         ]
      },
      {
         "StringValue":"Sir",
         "IntValue":30,
         "NestedArray":[
            {
               "FloatValue":4.09999990463257
            },
            {
               "FloatValue":5.19999980926514
            }
         ]
      }
   ],
   "Stuff":[
      "woop",
      "poow"
   ],
   "AllTypes":{
      "ByteValue":255,
      "ShortValue":32767,
      "IntValue":2147483647,
      "LongValue":"9223372036854775807",
      "GuidValue":"9cd0c2ea-dee4-4e0c-b703-bce71591e0c6",
      "DateTimeValue":"3155378975999999999",
      "Vector2Value":{
         "x":1,
         "y":1
      },
      "Vector3Value":{
         "x":1,
         "y":1,
         "z":1
      },
      "Vector4Value":{
         "x":1,
         "y":1,
         "z":1,
         "w":1
      },
      "QuaternionValue":{
         "x":1,
         "y":1,
         "z":1,
         "w":1
      },
      "SomeType":"Known"
   },
   "SimpleDictionary":[
      {
         "Key":"key1",
         "Value":"some-value"
      },
      {
         "Key":"key2",
         "Value":"some-other-value"
      }
   ],
   "ComplexDictionary":[
      {
         "Key":{
            "IntValue":10
         },
         "Value":{
            "FloatValue":32.2000007629395
         }
      }
   ],
   "Type":"Assets.Tests.Editor.A"
}
```

---

## Binary
This is about 270 bytes in size when in a file

```
15-41-73-73-65-74-73-2E-54-65-73-74-73-2E-45-64-69-74-6F-72-2E-41-03-57-4F-57-05-48-65-6C-6C-6F-00-00-00-00-01-00-00-00-1F-85-1B-40-02-00-00-00-05-54-68-65-72-65-14-00-00-00-01-00-00-00-00-00-60-40-03-53-69-72-1E-00-00-00-02-00-00-00-33-33-83-40-66-66-A6-40-02-00-00-00-04-77-6F-6F-70-04-70-6F-6F-77-FF-FF-7F-FF-FF-FF-7F-FF-FF-FF-FF-FF-FF-FF-7F-24-33-64-38-33-35-61-37-39-2D-62-61-33-33-2D-34-35-63-32-2D-38-30-32-65-2D-66-37-65-39-38-32-31-65-61-36-38-34-FF-3F-37-F4-75-28-CA-2B-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-01-00-00-00-02-00-00-00-04-6B-65-79-31-0A-73-6F-6D-65-2D-76-61-6C-75-65-04-6B-65-79-32-10-73-6F-6D-65-2D-6F-74-68-65-72-2D-76-61-6C-75-65-01-00-00-00-0A-00-00-00-CD-CC-00-42
```

---

## Xml
This is about 2.4kb when in a file
```xml
<Container>
  <TestValue>WOW</TestValue>
  <NestedValue>
    <StringValue>Hello</StringValue>
    <IntValue>0</IntValue>
    <NestedArray Count="1">
      <CollectionElement>
        <FloatValue>2.43</FloatValue>
      </CollectionElement>
    </NestedArray>
  </NestedValue>
  <NestedArray Count="2">
    <CollectionElement>
      <StringValue>There</StringValue>
      <IntValue>20</IntValue>
      <NestedArray Count="1">
        <CollectionElement>
          <FloatValue>3.5</FloatValue>
        </CollectionElement>
      </NestedArray>
    </CollectionElement>
    <CollectionElement>
      <StringValue>Sir</StringValue>
      <IntValue>30</IntValue>
      <NestedArray Count="2">
        <CollectionElement>
          <FloatValue>4.1</FloatValue>
        </CollectionElement>
        <CollectionElement>
          <FloatValue>5.2</FloatValue>
        </CollectionElement>
      </NestedArray>
    </CollectionElement>
  </NestedArray>
  <Stuff Count="2">
    <CollectionElement>woop</CollectionElement>
    <CollectionElement>poow</CollectionElement>
  </Stuff>
  <AllTypes>
    <ByteValue>255</ByteValue>
    <ShortValue>32767</ShortValue>
    <IntValue>2147483647</IntValue>
    <LongValue>9223372036854775807</LongValue>
    <GuidValue>216495ac-1e4e-412c-b6d7-071539446b76</GuidValue>
    <DateTimeValue>3155378975999999999</DateTimeValue>
    <Vector2Value>
      <x>1</x>
      <y>1</y>
    </Vector2Value>
    <Vector3Value>
      <x>1</x>
      <y>1</y>
      <z>1</z>
    </Vector3Value>
    <Vector4Value>
      <x>1</x>
      <y>1</y>
      <z>1</z>
      <w>1</w>
    </Vector4Value>
    <QuaternionValue>
      <x>1</x>
      <y>1</y>
      <z>1</z>
      <w>1</w>
    </QuaternionValue>
    <SomeType>Known</SomeType>
  </AllTypes>
  <SimpleDictionary Count="2">
    <KeyValuePair>
      <Key>key1</Key>
      <Value>some-value</Value>
    </KeyValuePair>
    <KeyValuePair>
      <Key>key2</Key>
      <Value>some-other-value</Value>
    </KeyValuePair>
  </SimpleDictionary>
  <ComplexDictionary Count="1">
    <KeyValuePair>
      <Key>
        <IntValue>10</IntValue>
      </Key>
      <Value>
        <FloatValue>32.2</FloatValue>
      </Value>
    </KeyValuePair>
  </ComplexDictionary>
  <Type>Assets.Tests.Editor.A</Type>
</Container>
```

---

As you can see the binary version is super efficient at about 1/4 that of JSON and 1/9 the size of XML, however its not human editable or readable. So in some cases it may make more sense to use JSON or support XML from webservices etc.