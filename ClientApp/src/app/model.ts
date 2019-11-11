export class Good {
    constructor(
        public id: string,
        public name: string,
        public price: number,
        public qty: number,
        public fileLink: FileLink,
        public categoryId: string
    ) { 
        if(fileLink == null)
            this.fileLink = new FileLink(null, null);
    }
}

export class Category {
    constructor(
        public id: string,
        public name: string,
        public childs: Category[]
    ) {}
}

export class FileLink {
    constructor(
        public id: string,
        public fileUrl: string,
    ) {}
}
