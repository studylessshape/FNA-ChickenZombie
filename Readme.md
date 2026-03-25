# FNA Template

## Clone this repo

```
git clone https://github.com/studylessshape/FNA-template --recursive
```

## Download fnalibs

Enter fnalibs action page: https://github.com/FNA-XNA/fnalibs-dailies/actions.

Then download CI artifacts and decompressed in [fnalibs](./fnalibs) folder.

## Build project

### Windows

- `Debug` or `Run`: use Visual Studio or dotnet-cli by `dotnet run`
- `Publish`: use Visual Studio or dotnet-cli by `dotnet publish`

### Linux

**Need Install**
1. `clang` or `gcc`;
2. `libsdl3`: `libsdl3-dev`,`libsdl3-ttf-dev`,`libsdl3-image-dev`

> If use `sdl2`, replace the `sdl3` to `sdl2`

- `Debug` or `Run`: use Visual Studio or dotnet-cli by `dotnet run`
- `Publish`: use Visual Studio or dotnet-cli by `dotnet publish`