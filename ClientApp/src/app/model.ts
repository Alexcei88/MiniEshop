export class Good {
    constructor(
        public id: string,
        public name: string,
        public price: number,
        public qty: number,
        public imageUrl: string,
        public categoryId: string
    ) { }
}

export class Category {
    constructor(
        public id: string,
        public name: string,
        public childs: Category[]
    ) {}
}
