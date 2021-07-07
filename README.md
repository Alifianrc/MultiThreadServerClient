# Multi Thread Server Client

This is socket programming project to simulate chatting with a server and several client using multi thread.

## Installation

1. Clone the repo
2. In [This File](/ServerMultiThread/ProcessClient.cs) change 
```
string filePath = @"D:\Tugas_Kuliah\Sistem_Oprasi_Jaringan_Komputer\FP_MultiClient\ServerMultiThread\Massage_Data.txt";
```
   to any path of the [Massage_Data.txt](/ServerMultiThread) file

## Chat Flow
1. User (in client) input message
2. Client send message to server
3. Server receive message
4. Server save message in txt file
5. Server send message to all client (including sender)
6. Client receive message

## Flowchart Diagram
![Flowchart](https://user-images.githubusercontent.com/62532983/124827963-d9fe1180-dfa0-11eb-95ff-5d7fe3f2a358.jpg)

## Result
![SS](https://user-images.githubusercontent.com/62532983/121777541-edde6f80-cbbc-11eb-99e2-5e521fed9b63.png)


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Contact
Mohammad Alifian - alifianrc@gmail.com
