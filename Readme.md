# FNA-ChickenZombie

Game from [Voidmatrix](https://space.bilibili.com/25864506)'s video https://space.bilibili.com/25864506/lists/4191853?type=season

## Clone this repo

```sh
git clone --recursive
```

## How to learn

View the git commit and step to watch the bilibili video.

## About Content/Resource

Please download from the summary of Voidmatrix's video.

And one more step: use `ffmpeg` turn **mp3** to **ogg**:

```sh
ffmpeg -i Content/bgm.mp3 Content/bgm.ogg
ffmpeg -i Content/loss.mp3 Content/loss.ogg
```

> Because FNA only support ogg song officially. You can try [NAudio](https://github.com/naudio/NAudio) to play mp3.

## How to build

See [FNA-template/Readme.md](https://github.com/studylessshape/FNA-template/blob/master/Readme.md)

## Some notice

1. Use `GraphicsDeviceManager` to set the window size;
2. Render texture by [Camera](./src/ChickenZombie/Engine/Camera.cs) use int and not has float;
3. Some coordinate is different with Voidmatrix's version;
4. Render camera first and other second because can't call `SpriteBatch.Begin()` twice;
5. ~~I didn't write some of the notes.~~