// import * as pixi from 'pixi.js';

// export class MoleculeBackground {
//     private renderer: pixi.WebGLRenderer | pixi.CanvasRenderer;
//     private height: number = window.innerHeight - 38;
//     private width: number = window.innerWidth;
//     private loader = pixi.loader;
//     private resources = pixi.loader.resources;
//     private stage: pixi.Container;


//     constructor(private container: HTMLElement) {


//         this.renderer = pixi.autoDetectRenderer(container.clientWidth, container.clientHeight, { antialias: false, transparent: true, resolution: 1 });

//         this.renderer.autoResize = true;
//         this.renderer.resize(container.clientWidth, container.clientHeight);
//         console.log(container.clientWidth, container.clientHeight);
//         container.appendChild(this.renderer.view);
//         this.stage = new pixi.Container();
        
//         this.loadSvg();
//     }

//     private loadSvg() {
//         pixi.loader.add('molecule', 'src/images/water_molecule.svg').load(() => this.setup());
//     }

//     private setup() {
//         const molecule = new pixi.Sprite(pixi.loader.resources['molecule'].texture);
//         this.stage.addChild(molecule);
//         this.renderer.render(this.stage);
//     }
// }

// export class MoleculeBackground {
//     private canvas: fabric.IStaticCanvas;
//     private height: number = window.innerHeight - 38;
//     private width: number = window.innerWidth;
//     private fpsTarget: number = 60;
//     private maxMolecules: number = 60; //math.random?
//     private molecules: fabric.IImage[] = new Array(this.maxMolecules);
//     private startTime;
//     private prevTime;
//     private ms;
//     private frames;
//     private updateTime = 1000 / this.fpsTarget;
//     constructor() {
//         this.canvas = new fabric.StaticCanvas('molecule-background');
//         this.canvas.setDimensions({height: this.height, width: this.width});

//         this.loadSvg();

//         //TODO - Optimize Resize
//         window.addEventListener('resize', () => {
//             this.onResize();
//         })
//     }


//     private onResize() {
//         this.height = window.innerHeight - 38;
//         this.width = window.innerWidth;
//         this.canvas.setDimensions({ height: this.height, width: this.width});
//     }

//     private loadSvg() {
//         fabric.Image.fromURL('src/assets/images/water_molecule.svg', (svg) => this.onSvgLoaded(svg));
//     }

//     private onSvgLoaded(svg) {
//         for(let i = 0; i < this.maxMolecules; i++) {
//             let img = new fabric.Image(svg.getElement(), {
//                 left: Math.random() * this.width,
//                 top: Math.random() * this.height,
//                 angle: Math.random() * 360,
//                 selectable: false
//             });

//             this.canvas.add(img);
//             this.molecules[i] = img;
//         }
//         this.render();
//         this.animate();
//     }

//     private animate() {
//         for(let i = 0; i < this.maxMolecules; i++) {
//             let molecule = this.molecules[i];

//             molecule.animate({ 'top': this.generateRandomAnimation(), 'left': this.generateRandomAnimation(), 'angle': this.generateRandomAnimation() }, 
//             {
//                 duration: 10000,
//                 easing: (t, b, c, d) => {return c*t/d + b;}
//             });

//             if(i == this.maxMolecules-1) {
//                 molecule.animate({ 'top': this.generateRandomAnimation(), 'left': this.generateRandomAnimation(), 'angle': this.generateRandomAnimation() }, {
//                     duration: 10000,
//                     onComplete: () => {
//                         this.animate();
//                     },
//                     easing: (t, b, c, d) => {return c*t/d + b;}
//                 });
//             }
            
//         }
//     }

//     private render() {
//         this.canvas.renderAll();
//         fabric.util.requestAnimFrame(() => this.render());
//     }

//     private generateRandomAnimation(): string {
//         let sign = ['+=', '-='];
//         let valueSign = ['+', '-'];

//         let animationValue = valueSign[Math.round(Math.random())] + Math.random() * 100;
//         let animationSign = sign[Math.round(Math.random())];
//         let animationString = animationValue.toString() + animationSign.toString();

//         return animationString;
//     }
// }