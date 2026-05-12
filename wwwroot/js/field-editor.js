class FieldEditor {
    static init(containerId, dotNetRef) {
        if (window.Sortable) {
            const el = document.getElementById(containerId);
            if (el) {
                if (this.sortable) {
                    this.sortable.destroy();
                }
                this.sortable = new Sortable(el, {
                    handle: '.drag-handle',
                    animation: 150,
                    onEnd: function (evt) {
                        const items = el.querySelectorAll('.field-block');
                        const order = Array.from(items).map((item, index) => ({
                            id: parseInt(item.dataset.fieldId),
                            order: index
                        }));
                        dotNetRef.invokeMethodAsync('ReorderFields', order);
                    }
                });
            }
        }
    }

    static destroy() {
        if (this.sortable) {
            this.sortable.destroy();
            this.sortable = null;
        }
    }
}
