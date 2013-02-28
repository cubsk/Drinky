
Ext.application({
    name: 'Drinky',
    requires: ['Ext.container.Viewport'],
    controllers: [
        'MainToolbar'
    ],
    launch: function () {
        Ext.create('Ext.container.Viewport', {
            //layout: 'fit',
            items: [
                {
                    xtype: 'panel',
                    title: 'Howdy!',
                    html: 'Cool new app goes here'
                },
                {
                    xtype: 'panel',
                    title: 'Another panel!',
                    html: 'Second panel'
                }
            ]
        });
    }
});